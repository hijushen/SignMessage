using NexChip.SignMessage.Bussiness.Enum;
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


        //更新
        public BizResult<List<SignMessageBox>> PostUpdateSignMsg(SignMessageSendDto msg, string appOID)
        {
            //NewOA.Employee emp = NewOA.Employee.GetEmployee("", "cocoge");


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
                        handleresult = 1,
                        updatetime = DateTime.Now,
                        handlemsgoids = msg.msgbody.boxOIDs //检查过存在
                    }).UpdateColumns(t => new { t.handleresult, t.handlemsgoids, t.updatetime }).ExecuteCommand();


                    //messageboxService.db.Updateable(new SignMessageBox
                    //{
                    //    updatetime = DateTime.Now,
                    //    emergencylevel = msg.msgbody.emergencylevel ?? 1,
                    //    callbackurl = msg.msgbody.callbackurl,
                    //    msgsourceid = msg.msgbody.msgsourceid
                    //})
                    //.WhereColumns(t => new { t.msgsourceid})
                    //.ExecuteCommand();

                    messageboxService.db.Updateable(new SignMessageBox
                    {
                        updatetime = DateTime.Now,
                        emergencylevel = msg.msgbody.emergencylevel ?? 1,
                        callbackurl = msg.msgbody.callbackurl,
                        msghandlestatus = HandleStatusString.Done,
                    }).Where(t => t.msgsourceid == msg.msgbody.msgsourceid)
                         .UpdateColumns(t => new
                         {
                             t.updatetime,
                             t.emergencylevel,
                             t.callbackurl,
                             t.msghandlestatus
                         }).ExecuteCommand();

                    messageboxService.db.CommitTran();
                    return new BizResult<List<SignMessageBox>>
                    {
                        Success = true
                    };
                }
                catch (Exception ex)
                {

                    messageboxService.db.RollbackTran();
                    return new BizResult<List<SignMessageBox>>
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


        //更新
        public BizResult<List<SignMessageBox>> PostUpdateNotifySignMsg(SignMessageSendDto msg, string appOID)
        {
            //NewOA.Employee emp = NewOA.Employee.GetEmployee("", "cocoge");


            try
            {
                if (msg == null)
                {
                    return interfaceHandlerErrorReturn("接收类型为空，请检查");
                }

                msg.sendtime = msg.sendtime ?? DateTime.Now;

                var res = this.checkPostUpdateNotifyData(msg, appOID);
                if (!res.Success) return res;

                var needUpdateRows = res.Data;
                foreach (var item in needUpdateRows)
                {
                    item.updatetime = DateTime.Now;
                    item.msghandlestatus = HandleStatusString.Done;
                }

                try
                {
                    messageboxService.db.BeginTran();
                    messageboxService.db.Updateable(new SignMessageInterface
                    {
                        OID = msg.interfaceOID,
                        handleresult = 1,
                        updatetime = DateTime.Now,
                        handlemsgoids = msg.msgbody.boxOIDs //检查过存在
                    }).UpdateColumns(t => new { t.handleresult, t.handlemsgoids }).ExecuteCommand();

                    messageboxService.db.Updateable(needUpdateRows)
                        .UpdateColumns(t => new { t.updatetime, t.msghandlestatus })
                        .ExecuteCommand();

                    #region 一样的错误方式
                    //messageboxService.db.Updateable<SignMessageBox>()
                    //    .SetColumns(t => new SignMessageBox()
                    //    {
                    //        updatetime = DateTime.Now,
                    //        msghandlestatus = "已完成"
                    //    })
                    // .Where("t.OID in (@ids)", new { ids = msg.msgbody.boxOIDs }).ExecuteCommand();

                    //messageboxService.db.Updateable(new SignMessageBox
                    //{
                    //    updatetime = DateTime.Now,
                    //    //emergencylevel = msg.msgbody.emergencylevel ?? 1,
                    //    //callbackurl = msg.msgbody.callbackurl,
                    //    msghandlestatus = "已完成"
                    //}).Where("t.OID in (@ids)",new {ids = msg.msgbody.boxOIDs })
                    //     .UpdateColumns(t => new
                    //     {
                    //         t.updatetime,
                    //         t.msghandlestatus
                    //         //t.emergencylevel,
                    //         //t.callbackurl
                    //     }).ExecuteCommand();
                    #endregion


                    messageboxService.db.CommitTran();
                    return new BizResult<List<SignMessageBox>>
                    {
                        Success = true
                    };
                }
                catch (Exception ex)
                {

                    messageboxService.db.RollbackTran();
                    return new BizResult<List<SignMessageBox>>
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


        private List<SignMessageBox> geneInsertSignMessageBox(SignMessageSendDto msg)
        {
            List<SignMessageBox> lstSignMessageBox = new List<SignMessageBox>();

            var ids = msg.msgbody.toids.Split(",");
            var names = msg.msgbody.toids.Split(",");

            for (int i = 0; i < ids.Length; i++)
            {
                SignMessageBox saveEntity = new SignMessageBox
                {
                    OID = Guid.NewGuid().ToString(),
                    appname = msg.appname,
                    msgsourceid = msg.msgbody.msgsourceid,
                    sendtime = msg.sendtime,
                    fromempid = msg.msgbody.fromid.ToUpper(),
                    fromempname = msg.msgbody.fromname,
                    toempid = ids[i].ToUpper(),
                    toempname = names[i],
                    callbackurl = msg.msgbody.callbackurl,
                    substance = msg.msgbody.substance,
                    showmsg = msg.msgbody.showmsg,
                    createtime = DateTime.Now,
                    msgstatus = 0, //未读
                    msghandlestatus = HandleStatusString.Undo,
                    emergencylevel = msg.msgbody.emergencylevel ?? 1
                };

                lstSignMessageBox.Add(saveEntity);
            }

            return lstSignMessageBox;
        }

        private string getMessageBoxIds(List<SignMessageBox> boxEntities)
        {
            StringBuilder strRet = new StringBuilder();
            foreach (var item in boxEntities)
            {
                strRet.Append(item.OID);
                strRet.Append(",");
            }
            var str = strRet.ToString().Substring(0, strRet.Length - 1);
            str = str.Substring(0, 4000); //防止超过数据库长度，超过4000日志不记录.
            return str;
        }


        //新增
        public BizResult<List<SignMessageBox>> PostNewSignMsg(SignMessageSendDto msg, string appOID)
        {
            try
            {
                if (msg == null)
                {
                    return new BizResult<List<SignMessageBox>>
                    {
                        Success = false,
                        Msg = "接收类型为空，请检查"
                    };
                }

                msg.sendtime = msg.sendtime ?? DateTime.Now;

                var res = checkPostAddData(msg, appOID);
                if (!res.Success) return res;

                var saveEntities = this.geneInsertSignMessageBox(msg);

                try
                {
                    messageboxService.db.BeginTran();

                    foreach (var saveEntity in saveEntities)
                    {
                        messageboxService.InsertIgnoreNullColumn(saveEntity);
                    }

                    messageboxService.db.Updateable(new SignMessageInterface
                    {
                        OID = msg.interfaceOID,
                        handleresult = 1,
                        handlemsgoids = getMessageBoxIds(saveEntities),
                        updatetime = DateTime.Now
                    })
                    .UpdateColumns(t => new { t.handleresult, t.updatetime, t.handlemsgoids }).ExecuteCommand();

                    messageboxService.db.CommitTran();
                    return new BizResult<List<SignMessageBox>>
                    {
                        Success = true,
                        Data = saveEntities
                    };

                }
                catch (Exception ex)
                {
                    messageboxService.db.RollbackTran();
                    return new BizResult<List<SignMessageBox>>
                    {
                        Success = false,
                        Msg = ex.Message
                    };
                }

            }
            catch (Exception ex)
            {
                return new BizResult<List<SignMessageBox>>
                {
                    Success = false,
                    Msg = ex.Message
                };
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
                    createtime = DateTime.Now,
                    handleresult = handleResult,
                    handleerrormsg = msg.handleerrormsg ?? "",
                    handlemsgoids = msg.msgbody.boxOIDs ?? "",
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

        /// <summary>
        /// 检查消息完整性，角色有效期，数据非空项，接口表数据新增
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="appOID"></param>
        /// <returns></returns>
        private BizResult<List<SignMessageBox>> checkCommonData(SignMessageSendDto msg, string appOID)
        {
            string appN = "";
            var roleEntity = roleService.sdb.GetSingle<SignMessageRole>(t => t.OID == appOID);
            if (roleEntity != null)
            {
                appN = roleEntity.appname;
            }

            if (string.IsNullOrEmpty(msg.appname))
            {
                msg.handleerrormsg = "请检查数据：发送appname不能为空！";
                msg.appname = string.IsNullOrWhiteSpace(appN) ? appOID : appN;
                insertMsgInterface(msg, 0); //1. 插入接口表失败记录
                return interfaceHandlerErrorReturn(msg.handleerrormsg);
            }

            insertMsgInterface(msg, -1); //1. 插入接口表中，待处理
            if (msg.interfaceOID == "")
            {
                msg.handleerrormsg = "请联系平台,错误信息： 接口表插入异常！";
                return interfaceHandlerErrorReturn(msg.handleerrormsg);
            }

            //2. 检查角色是否有效，拦截
            var chkrole = this.checkRoleInfo(msg, roleEntity);
            if (!chkrole.Success) return chkrole;

            //3. 检查本身内容完整性
            var chkMsgBody = this.checkMsgInfo(msg);
            return chkMsgBody;

        }
        private BizResult<List<SignMessageBox>> checkPostAddData(SignMessageSendDto msg, string appOID)
        {

            var chkComm = this.checkCommonData(msg, appOID);


            if (msg.msgbody.handletype != (int)HandleTypeEnum.Add)
            {
                msg.handleerrormsg = "检查数据。消息处理类型错误";
                updateMsgInterfaceErrorHandle(msg);
                return new BizResult<List<SignMessageBox>>
                {
                    Success = false,
                    Msg = msg.handleerrormsg
                };
            }

            return chkComm;
        }

        /// <summary>
        /// 检查更新类消息完整性
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="appOID"></param>
        /// <returns></returns>
        private BizResult<List<SignMessageBox>> checkPostUpdateData(SignMessageSendDto msg, string appOID)
        {

            var chkComm = this.checkCommonData(msg, appOID);
            if (!chkComm.Success) return chkComm;

            var existEntities = messageboxService.sdb.GetList<SignMessageBox>(t => t.msgsourceid == msg.msgbody.msgsourceid);
            if (existEntities.Count == 0)
            {
                msg.handleerrormsg = "提供msgsourceid找不到对应消息，请检查";
                updateMsgInterfaceErrorHandle(msg);
                return new BizResult<List<SignMessageBox>>
                {
                    Success = false,
                    Msg = msg.handleerrormsg
                };

            }
            else
            {
                msg.msgbody.boxOIDs = getMessageBoxIds(existEntities);
            }

            var types = new int[] { (int)HandleTypeEnum.Completed, (int)HandleTypeEnum.Del };
            if (Array.IndexOf(types, msg.msgbody.handletype) == -1)
            {
                msg.handleerrormsg = "检查数据。消息处理类型错误";
                updateMsgInterfaceErrorHandle(msg);
                return new BizResult<List<SignMessageBox>>
                {
                    Success = false,
                    Msg = msg.handleerrormsg
                };
            }


            return new BizResult<List<SignMessageBox>>
            {
                Success = true
            };
        }


        /// <summary>
        /// 检查更新通知类消息完整性
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="appOID"></param>
        /// <returns></returns>
        private BizResult<List<SignMessageBox>> checkPostUpdateNotifyData(SignMessageSendDto msg, string appOID)
        {

            var chkComm = this.checkCommonData(msg, appOID);
            if (!chkComm.Success) return chkComm;

            List<SignMessageBox> existMsgBoxEntities = new List<SignMessageBox>();
            existMsgBoxEntities = messageboxService.db.Queryable<SignMessageBox>("t")
                .Where(t => t.appname == msg.appname && t.createtime <= msg.sendtime)
                .Where("t.toempid in (@ids)", new { ids = msg.msgbody.toids }).ToList();

            if (existMsgBoxEntities.Count == 0)
            {
                msg.handleerrormsg = "无需要更新消息";
                updateMsgInterfaceErrorHandle(msg);
                return new BizResult<List<SignMessageBox>>
                {
                    Success = false,
                    Msg = msg.handleerrormsg
                };

            }
            else
            {
                msg.msgbody.boxOIDs = getMessageBoxIds(existMsgBoxEntities);
            }

            return new BizResult<List<SignMessageBox>>
            {
                Success = true,
                Data = existMsgBoxEntities
            };
        }


        private BizResult<List<SignMessageBox>> checkMsgInfo(SignMessageSendDto msg)
        {
            var msgbody = msg.msgbody;

            #region 为空检测
            StringBuilder errMsg = new StringBuilder();

            if (string.IsNullOrWhiteSpace(msgbody.msgsourceid))
            {
                errMsg.Append("msgsourceid 不能为空;");
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

            if (!string.IsNullOrWhiteSpace(msgbody.toids) && !string.IsNullOrWhiteSpace(msgbody.tonames))
            {
                if (msgbody.toids.Split(",").Length != msgbody.tonames.Split(",").Length)
                {
                    errMsg.Append("toids 和 tonames 数量不符");
                }
            }

            //全部大写的工号
            msg.msgbody.fromid = msg.msgbody.fromid.ToUpper();
            msg.msgbody.toids = msg.msgbody.toids.ToUpper();

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

                return new BizResult<List<SignMessageBox>>
                {
                    Success = false,
                    Msg = errMsgRes
                };
            }
            #endregion


            msgbody.emergencylevel = msgbody.emergencylevel ?? 1;
            msgbody.substance = msgbody.substance ?? msg.appname;

            return new BizResult<List<SignMessageBox>>
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
        private BizResult<List<SignMessageBox>> checkRoleInfo(SignMessageSendDto msg, SignMessageRole roleEntity)
        {
            if (msg.appname != roleEntity.appname)
            {
                msg.handleerrormsg = "请检查数据: 用户身份与appname不一致！";
                updateMsgInterfaceErrorHandle(msg);
                return interfaceHandlerErrorReturn(msg.handleerrormsg);
            }

            if (roleEntity.rolestatus == 0)
            {
                msg.handleerrormsg = "请联系平台，用户身份已失效！";
                updateMsgInterfaceErrorHandle(msg);
                return interfaceHandlerErrorReturn(msg.handleerrormsg);
            }

            return new BizResult<List<SignMessageBox>>
            {
                Success = true
            };
        }

        #endregion

        private BizResult<List<SignMessageBox>> interfaceHandlerErrorReturn(string msg)
        {
            return new BizResult<List<SignMessageBox>>
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
