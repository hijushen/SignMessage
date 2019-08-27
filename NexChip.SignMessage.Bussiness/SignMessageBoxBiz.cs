using NexChip.SignMessage.Bussiness.Models.Dtos;
using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Services;
using NexChip.SignMessage.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    public class SignMessageBoxBiz
    {
        private SignMessageBoxService Service = new SignMessageBoxService();

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

        public BizListResultForDataTables<SignMessageBox> ListForDataTables(DataTablesRequsetDto reqP)
        {
            int ordertype = 0;
            int limit = reqP.length;
            int start = reqP.start;

            if (string.IsNullOrEmpty(reqP.orderString)) //排序方式为空， 倒序
            {
                ordertype = 1;
            }

            Expression<Func<SignMessageBox, bool>> whereExpression = null;
            Expression<Func<SignMessageBox, object>> orderByExpression = p => p.createtime;

            var res = Service.GetPageList(start, limit, orderByExpression, ordertype, whereExpression);
            return new BizListResultForDataTables<SignMessageBox>
            {
                data = res.Rows,
                draw = reqP.draw + 1,
                recordsFiltered = res.total,
                recordsTotal = res.total
            };
        }

        public  BizResult<SignMessageBoxDto> testSend(string OID)
        {
            try
            {
                SignMessageBoxDto dto = new SignMessageBoxDto();

                var signBox = Service.sdb.GetSingle<SignMessageBox>(t => t.OID == OID);
                if(signBox == null)
                {
                    return new BizResult<SignMessageBoxDto>
                    {
                        Success = false,
                        Msg = "未找到，请重试"
                    };
                }

                var postUrl = "/SignMessage/UpdateSignMessage";
                var postData = new SignMessageSendDto
                {
                    appname = "23",
                    sendtime = DateTime.Now,
                    msgbody = new SignMessageSendBodyDto
                    {
                        sourceid = signBox.OID,
                        callbackurl = "http://www.baidu.com",
                        fromid = signBox.fromempid,
                        toids = signBox.toempid,
                        fromname = signBox.fromempname,
                        tonames = signBox.toempname,
                        handletype = 1,
                        emergencylevel = 1
                        ,msgsourceid = signBox.msgsourceid
                    }
                };

                var response = RestSharpHttp.PostJson(postUrl, postData.SerializeModel());

                //var response = HttpClinetHelper.PostAsyncJson(postUrl, postData.SerializeModel())
                //    .ConfigureAwait(false).GetAwaiter().GetResult();


                var returnD = response.DeserializeModel<BizResult<SignMessageBoxDto>>();

                return returnD;
                //return new BizResult<SignMessageBoxDto>
                //{
                //    Success = true,
                //    Data = returnD
                //};
            }
            catch(Exception ex)
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
