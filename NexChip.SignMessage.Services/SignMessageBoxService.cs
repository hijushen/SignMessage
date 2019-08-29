using NexChip.SignMessage.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NexChip.SignMessage.Services
{
    public class SignMessageBoxService : BaseService<SignMessageBox>
    {
        private string builderHanderStatus(string handleStatus)
        {
            if (string.IsNullOrWhiteSpace(handleStatus)) return handleStatus;

            string[] status = handleStatus.Split(",");
            StringBuilder sb = new StringBuilder();
            foreach (var item in status)
            {
                if(item.Trim().Length > 0)
                {
                    sb.Append("'");
                    sb.Append(item);
                    sb.Append("',");
                }
            }

            return sb.ToString(0, sb.Length - 1);

        }

        public BizListResult<SignMessageBox> GetPageList(int pageIndex , int pageSize, string userName
            ,DateTime startD, DateTime endD, string formType, string handleStatus)
        {
            
            int totalCount = 0;
            PageModel p = new PageModel() { PageIndex = pageIndex, PageSize = pageSize };

            //var data = db.Queryable<SignMessageBox>()
            //    .Where(s => s.toempname == userName)
            //    //.Where("msghandlestatus in ('@hstatus') ",new { hstatus = handleStatus})
            //    .Where(s => s.sendtime >= startD && s.sendtime <= endD)                
            //    .WhereIF(formType !="所有", s => s.appname == formType)
            //    .ToPageList(pageIndex, pageSize, ref totalCount);

            //var exp = Expressionable.Create<SignMessageBox>()
            //    .And(s => s.toempname == userName)
            //    .And(s => s.sendtime >= startD)
            //    .And(s => s.sendtime <= endD)
            //    .AndIF(formType != "所有", s => s.appname == formType)
            //    .ToExpression();
            //var data = sdb.GetPageList(exp, p);

            var queryable = db.Queryable<SignMessageBox>("t")
                .Where(s => s.toempname == userName)
                .Where(s => s.sendtime >= startD && s.sendtime < endD.AddDays(1))
                .WhereIF(formType != "所有", s => s.appname == formType);

            //queryable.Where("t.msghandlestatus in (@status)", new { status = builderHanderStatus(handleStatus) });

            var data = queryable.ToPageList(pageIndex, pageSize, ref totalCount);


            var t = new BizListResult<SignMessageBox>
            {
                total = totalCount,
                Rows = data,
                Msg = "成功"
            };
            return t;
        }


    }
}
