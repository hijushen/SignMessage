using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    public class SignMessgeBiz
    {
        private SignMessageBoxService messageboxService = new SignMessageBoxService();
        private SignMessageRoleService roleService = new SignMessageRoleService();
        private SignMessageInterfaceService interfaceService = new SignMessageInterfaceService();

        private TempService tempService = new TempService();


        public DataTable getTest()
        {
            return roleService.TestList();
        }

        public void MockSignBoxDaa()
        {
            tempService.MockMessages();
        }

        public BizListResult<SignMessageBox> MessageList(int start, int limit, string sort = "", string order = "")
        {
            int ordertype = 0;
            if (order == "desc")
            {
                ordertype = 1;
            }

            Expression<Func<SignMessageBox, bool>> whereExpression = null;
            Expression<Func<SignMessageBox, object>> orderByExpression = p => p.remark;
            return messageboxService.GetPageList(start, limit, orderByExpression, ordertype, whereExpression);
        }


        public BizResult<SignMessageBox> PostUpdateSignMsg(SignMessageSendDto msg, string appOID)
        {
            try
            {
                if (msg == null)
                {
                    return interfaceHandlerErrorReturn("接收类型为空，请检查");
                }

                msg.sendtime = msg.sendtime ?? DateTime.Now;

                var res = this.checkPostUpdateData(msg, appOID);
                if (!res.Success) return res;


                try
                {
                    messageboxService.db.BeginTran();
                    messageboxService.db.Updateable(new SignMessageInterface
                    {
                        OID = msg.interfaceOID,
                        handleresult = 1
                    }).UpdateColumns(t => new { t.handleresult }).ExecuteCommand();

                    messageboxService.db.Updateable(new SignMessageBox
                    {
                        updatetime = DateTime.Now,
                        emergencylevel = msg.msgbody.emergencylevel ?? 1,
                        callbackurl = msg.msgbody.callbackurl
                    }).Where(t=> t.msgsourceid == msg.msgbody.sourceid)
                    .UpdateColumns(t => new {
                        t.updatetime, t.emergencylevel,t.callbackurl
                    }).ExecuteCommand();


                    messageboxService.db.CommitTran();
                    return new BizResult<SignMessageBox>
                    {
                        Success = true
                    };
                }
                catch (Exception ex)
                {

                    messageboxService.db.RollbackTran();
                    return new BizResult<SignMessageBox>
                    {
                        Success = false,
                        Msg = ex.Message
                    };
                }

            }
            catch (Exception ex)
            {
                return interfaceHandlerErrorReturn(ex.Message);
            }
        }


        public BizResult<SignMessageBox> PostNewSignMsg(SignMessageSendDto msg, string appOID)
        {
            try
            {
                if (msg == null)
                {
                    return interfaceHandlerErrorReturn("接收类型为空，请检查");
                }

                msg.sendtime = msg.sendtime ?? DateTime.Now;

                var res = checkPostAddData(msg, appOID);
                if (!res.Success) return res;

                SignMessageBox saveEntity = new SignMessageBox
                {
                    OID = Guid.NewGuid().ToString(),
                    appname = msg.appname,
                    msgsourceid = msg.msgbody.sourceid,
                    sendtime = msg.sendtime,
                    fromempid = msg.msgbody.fromid,
                    fromempname = msg.msgbody.fromname,
                    toempid = msg.msgbody.toids,
                    toempname = msg.msgbody.tonames,
                    callbackurl = msg.msgbody.callbackurl,
                    substance = msg.msgbody.substance,
                    showmsg = msg.msgbody.showmsg,
                    createtime = DateTime.Now,
                    msgstatus = 0, //未读
                    emergencylevel = msg.msgbody.emergencylevel ?? 1
                };

                try
                {
                    messageboxService.db.BeginTran();
                    messageboxService.InsertIgnoreNullColumn(saveEntity);
                    messageboxService.db.Updateable(new SignMessageInterface
                    {
                        OID = msg.interfaceOID,
                        handleresult = 1
                    }).UpdateColumns(t => new { t.handleresult }).ExecuteCommand();



                    messageboxService.db.CommitTran();
                    return new BizResult<SignMessageBox>
                    {
                        Success = true,
                        Data = saveEntity
                    };

                }
                catch(Exception ex)
                {
                    messageboxService.db.RollbackTran();
                    return new BizResult<SignMessageBox>
                    {
                        Success = false,
                        Msg = ex.Message
                    };
                }

            }
            catch (Exception ex)
            {
                return interfaceHandlerErrorReturn(ex.Message);
            }


        }

        /// <summary>
        /// 接受数据接口表信息生成
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="handleResult">当前状态类型(-1待处理， 0处理结果为错误）</param>
        /// <returns>接口表主键</returns>
        private void insertMsgInterface(SignMessageSendDto msg, int handleResult = -1)
        {
            try
            {
                SignMessageInterface face = new SignMessageInterface
                {
                    OID = Guid.NewGuid().ToString(),
                    appname = msg.appname,
                    sendtime = msg.sendtime ?? DateTime.Now,
                    handleresult = handleResult,
                    msgbody = msg.msgbody.SerializeModel()
                };

                var res = interfaceService.Insert(face);
                if (res)
                {
                    msg.interfaceOID = face.OID;
                }
                else
                {
                    msg.interfaceOID = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("insertMsgInterface Exception: " + ex.Message);
                msg.interfaceOID = "";
            }
        }

        private void updateMsgInterfaceErrorHandle(SignMessageSendDto msg)
        {
            SignMessageInterface face = new SignMessageInterface
            {
                OID = msg.interfaceOID,
                handleresult = 0,//msg.handleresult,
                handleerrormsg = msg.handleerrormsg
            };

            try
            {
                interfaceService.UpdateOnlyColumn(face, t => new { t.handleresult, t.handleerrormsg });
            }
            catch (Exception ex)
            {
                Console.WriteLine("updateMsgInterface Exception: " + ex.Message);
            }
        }

        #region 相关验证工作
        private BizResult<SignMessageBox> checkCommonData(SignMessageSendDto msg, string appOID)
        {

            if (string.IsNullOrEmpty(msg.appname))
            {
                msg.appname = appOID;
                insertMsgInterface(msg, 0); //1. 插入接口表失败记录
                return interfaceHandlerErrorReturn("请检查数据：发送appname不能为空！");
            }

            insertMsgInterface(msg); //1. 插入接口表中，待处理
            if (msg.interfaceOID == "")
            {
                return interfaceHandlerErrorReturn("请联系平台,错误信息： 接口表插入异常！");
            }


            var roleEntity = roleService.sdb.GetSingle<SignMessageRole>(t => t.OID == appOID);
            var chkrole = this.checkRoleInfo(msg, roleEntity);
            if (!chkrole.Success) return chkrole;


            var chkMsgBody = this.checkMsgInfo(msg);

            return chkMsgBody;
            
        }
        private BizResult<SignMessageBox> checkPostAddData(SignMessageSendDto msg, string appOID)
        {

            var chkComm = this.checkCommonData(msg, appOID);
            if (!chkComm.Success) return chkComm;
            
            return new BizResult<SignMessageBox>
            {
                Success = true
            };
        }

        private BizResult<SignMessageBox> checkPostUpdateData(SignMessageSendDto msg, string appOID)
        {

            var chkComm = this.checkCommonData(msg, appOID);
            if (!chkComm.Success) return chkComm;

            if(string.IsNullOrEmpty(msg.msgbody.msgsourceid))
            {
                msg.handleerrormsg = "更新需要原系统标识id";
                updateMsgInterfaceErrorHandle(msg);

                return new BizResult<SignMessageBox>
                {
                    Success = false,
                    Msg = msg.handleerrormsg
                };
            }


            return new BizResult<SignMessageBox>
            {
                Success = true
            };
        }


        private BizResult<SignMessageBox> checkMsgInfo(SignMessageSendDto msg)
        {
            var msgbody = msg.msgbody;

            #region 为空检测
            StringBuilder errMsg = new StringBuilder();

            if (string.IsNullOrWhiteSpace(msgbody.sourceid))
            {
                errMsg.Append("sourceid 不能为空;");
            }
            if (string.IsNullOrWhiteSpace(msgbody.fromid))
            {
                errMsg.Append("fromid 不能为空;");
            }
            if (string.IsNullOrWhiteSpace(msgbody.fromname))
            {
                errMsg.Append("fromname 不能为空;");
            }
            if (string.IsNullOrWhiteSpace(msgbody.toids))
            {
                errMsg.Append("toids 不能为空;");
            }
            if (string.IsNullOrWhiteSpace(msgbody.tonames))
            {
                errMsg.Append("tonames 不能为空;");
            }
            if (string.IsNullOrWhiteSpace(msgbody.handletype.ToString()))
            {
                errMsg.Append("handletype 不能为空;");
            }

            if (string.IsNullOrWhiteSpace(msgbody.callbackurl))
            {
                errMsg.Append("callbackurl 不能为空;");
            }

            string errMsgRes = errMsg.ToString();
            if (errMsgRes.Length > 3)
            {
                msg.handleerrormsg = errMsgRes;
                updateMsgInterfaceErrorHandle(msg);

                return new BizResult<SignMessageBox>
                {
                    Success = false,
                    Msg = errMsgRes
                };
            }
            #endregion


            msgbody.emergencylevel = msgbody.emergencylevel ?? 1;
            msgbody.substance = msgbody.substance ?? msg.appname;

            return new BizResult<SignMessageBox>
            {
                Success = true
            };
        }

        /// <summary>
        /// 检查用户角色--失效，过期等
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        private BizResult<SignMessageBox> checkRoleInfo(SignMessageSendDto msg, SignMessageRole roleEntity)
        {
            if (msg.appname != roleEntity.appname)
            {
                msg.handleerrormsg = "请检查数据: 用户身份与appname不一致！";
                updateMsgInterfaceErrorHandle(msg);
                return interfaceHandlerErrorReturn(msg.handleerrormsg);
            }

            if (roleEntity.rolestatus == 0)
            {
                msg.handleerrormsg = "请联系平台，用户身份权限已失效！";
                updateMsgInterfaceErrorHandle(msg);
                return interfaceHandlerErrorReturn(msg.handleerrormsg);
            }

            return new BizResult<SignMessageBox>
            {
                Success = true
            };
        }

        #endregion

        private BizResult<SignMessageBox> interfaceHandlerErrorReturn(string msg)
        {
            return new BizResult<SignMessageBox>
            {
                Success = false,
                Msg = msg
            };
        }


        //public object MessageList()
        //{
        //    //return msgRoleService.sdb.GetPageList<>
        //}
    }
}
