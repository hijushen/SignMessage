
using NexChip.SignMessage.Bussiness.Enum;
using NexChip.SignMessage.Bussiness.Models;
using NexChip.SignMessage.Bussiness.Models.Dtos;
using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Services;
using NexChip.SignMessage.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    public class SignMessageBoxBiz
    {
        private SignMessageBoxService Service = new SignMessageBoxService();
        private SignMessageRoleService roleService = new SignMessageRoleService();

        public object Delete(string[] oIDs)
        {
            throw new NotImplementedException();
        }

        public object EditSave(SignMessageRoleDto dto)
        {
            throw new NotImplementedException();
        }

        public object Get(string oID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新消息只读状态
        /// </summary>
        /// <param name="oID"></param>
        /// <returns></returns>
        public BizResult<SignMessageBox> UpdateRead(string oID)
        {
            try
            {
                int i = Service.UpdateOnlyColumn(new SignMessageBox { OID = oID, msgstatus = 1, updatetime = DateTime.Now }, it => new { it.msgstatus, it.updatetime });
                return new BizResult<SignMessageBox>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message, ex);
                return new BizResult<SignMessageBox>
                {
                    Success = false,
                    Msg = ex.Message
                };
            }
            
        }


        public Employee getUserInfo(string email)
        {
            DataTable dt = roleService.getEmployee(email);

            var json = dt.SerializeModel();
            var employees = json.DeserializeModel<List<Employee>>();
            if (employees.Count >= 1)
            {
                return employees[0];
            }
            else
            {
                throw new Exception("根据email查询人员信息异常");
            }
        }

        public List<string> GetDistinctFormNames()
        {
            var retListStr = new List<string>{ SettingConfig.AllString };            
            var dbTypes =  roleService.GetDistinctFormNames();
            retListStr.AddRange(dbTypes);

            return retListStr;
        }

        public BizListResultForDataTables<SignMessageBox> ListForDataTables(DataTablesRequsetDto reqP, string email)
        {
            var emp = getUserInfo(email);
            string toempid = emp.emp_id;//"TEST01";


            string formtype = SettingConfig.AllString;
            if(reqP.formtype != formtype)
            {
                formtype = roleService.getAppNameByChs(reqP.formtype);
            }

            int limit = reqP.length;
            int start = reqP.start;
            double page = start / limit;
            int pageIndex = Math.Ceiling(page).ObjToInt() + 1;//Sql Sugar 页从1开始！


            var timeSpan = reqP.timespan.Split("-");

            //零点日期
            var startD = new DateTime(timeSpan[0].ObjToInt(), timeSpan[1].ObjToInt(), timeSpan[2].ObjToInt());
            var endD = new DateTime(timeSpan[3].ObjToInt(), timeSpan[4].ObjToInt(), timeSpan[5].ObjToInt());


            var res = Service.GetPageList(pageIndex, limit, toempid, startD, endD, formtype, reqP.msgstatus, reqP.msghandlestatus);
            return new BizListResultForDataTables<SignMessageBox>
            {
                data = res.Rows,
                draw = reqP.draw + 1,
                recordsFiltered = res.total,
                recordsTotal = res.total,
                unReadCount = res.Code
            };

            //Expression<Func<SignMessageBox, object>> orderByExpression = p => p.createtime;
            //Expression<Func<SignMessageBox, bool>> whereExpression = buildWhereExpreesion(reqP, userName);

            //var res = Service.GetPageList(start, limit);
            //return new BizListResultForDataTables<SignMessageBox>
            //{
            //    data = res.Rows,
            //    draw = reqP.draw + 1,
            //    recordsFiltered = res.total,
            //    recordsTotal = res.total
            //};
        }

        public BizResult<SignMessageBox> GetUnReadCount(DataTablesRequsetDto reqP, string email)
        {
            var emp = getUserInfo(email);
            string toempid = emp.emp_id;//"TEST01";

            var timeSpan = reqP.timespan.Split("-");
            //零点日期
            var startD = new DateTime(timeSpan[0].ObjToInt(), timeSpan[1].ObjToInt(), timeSpan[2].ObjToInt());
            var endD = new DateTime(timeSpan[3].ObjToInt(), timeSpan[4].ObjToInt(), timeSpan[5].ObjToInt());

            string formtype = SettingConfig.AllString;
            if (reqP.formtype != formtype)
            {
                formtype = roleService.getAppNameByChs(reqP.formtype);
            }

            return Service.GetUnReadCount(toempid, startD, endD, formtype, HandleStatusString.Undo,(int)ReadStatusEnum.UnRead);
        }


        /// <summary>
        /// 消息测试-更新
        /// </summary>
        /// <param name="OID"></param>
        /// <param name="type">类型,1新增， 2更新</param>
        /// <returns></returns>
        public BizResult<SignMessageBoxDto> testSend(string OID, int type)
        {
            try
            {
                SignMessageBoxDto dto = new SignMessageBoxDto();

                var signBox = Service.sdb.GetSingle<SignMessageBox>(t => t.OID == OID);
                if (signBox == null)
                {
                    return new BizResult<SignMessageBoxDto>
                    {
                        Success = false,
                        Msg = "未找到，请重试"
                    };
                }

                string postUrl = "";
                if (type == 1)
                {
                    postUrl = "/SignMessage/UpdateSignMsg";
                }
                else
                {
                    postUrl = "/SignMessage/NewSignMsg";
                }
                //var getUrl = "/v1/Values";
                var postData = new SignMessageSendDto
                {
                    appname = signBox.appname,
                    sendtime = DateTime.Now,
                    msgbody =
                    type == 1 ? new SignMessageSendBodyDto  //更新
                    {
                        //sourceid = signBox.OID,
                        callbackurl = "http://www.baidu.com",
                        fromid = signBox.fromempid,
                        toids = signBox.toempid,
                        fromname = signBox.fromempname,
                        tonames = signBox.toempname,
                        handletype = type, //1，2，3 新增/更新/删除
                        emergencylevel = (int)EmergencyLevelEnum.Normal,
                        msgsourceid = signBox.msgsourceid //必要条件
                    }
                    : new SignMessageSendBodyDto  //新增
                    {
                        //sourceid = signBox.OID,
                        callbackurl = "http://www.baidu.com",
                        fromid = signBox.fromempid,
                        toids = signBox.toempid,
                        fromname = signBox.fromempname,
                        tonames = signBox.toempname,
                        handletype = type, //1，2，3 新增/更新/删除
                        emergencylevel = (int)EmergencyLevelEnum.Normal,
                        msgsourceid = signBox.msgsourceid //必要条件
                    }
                };

                //var response = new CallAPI().PostCall(postUrl, postData.SerializeModel()).Content;
                var response = RestSharpHttp.PostJson(postUrl, postData.SerializeModel());

                //var response = HttpClinetHelper.Post(postUrl, postData.SerializeModel());
                //var response = HttpClinetHelper.HttpPost(postUrl, postData.SerializeModel());

                   //.ConfigureAwait(false).GetAwaiter().GetResult();

                if (response == "")
                {
                    return new BizResult<SignMessageBoxDto>
                    {
                        Success = false,
                        Msg = "网络请求内容为空"
                    };
                }
                else
                {
                    var returnD = response.DeserializeModel<BizResult<SignMessageBoxDto>>();
                    return returnD;
                }
                
                //return new BizResult<SignMessageBoxDto>
                //{
                //    Success = true,
                //    Data = returnD
                //};
            }
            catch (Exception ex)
            {
                return new BizResult<SignMessageBoxDto>
                {
                    Success = false,
                    Msg = ex.Message
                };
            }
        }
    }
}
