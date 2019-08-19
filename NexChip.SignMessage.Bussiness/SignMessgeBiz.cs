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
        private SignMessageBoxService service = new SignMessageBoxService();
        private SignMessageRoleService msgRoleService = new SignMessageRoleService();
        private TempService tempService = new TempService();

        public DataTable getTest()
        {
            return msgRoleService.TestList();
        }

        public void MockSignBoxDaa()
        {
            tempService.MockMessages();
        }

        public BizListResult<SignMessageBox> MessageList(int start, int limit,string sort="", string order="")
        {
            int ordertype = 0;
            if(order == "desc")
            {
                ordertype = 1;
            }

            Expression<Func<SignMessageBox, bool>> whereExpression = null;
            Expression<Func<SignMessageBox, object>> orderByExpression = p => p.remark;
            return service.GetPageList(start, limit, orderByExpression,ordertype,whereExpression);
        }

        //public object MessageList()
        //{
        //    //return msgRoleService.sdb.GetPageList<>
        //}
    }
}
