using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Utils;
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

        public BizListResult<SignMessageBox> GetPageList(int pageIndex , int pageSize, string toempid
            ,DateTime startD, DateTime endD, string formType, string msgStatus,string msgHandleStatus)
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

            var status = msgStatus == "未读" ? 0 : 1;

            var queryable = db.Queryable<SignMessageBox, SignMessageRole>(
                (s, sc) => new JoinQueryInfos(JoinType.Inner, s.appname == sc.appname))
                .Where(s => s.toempid == toempid)
                .Where(s => s.sendtime >= startD && s.sendtime < endD.AddDays(1))
                .Where(s => s.msghandlestatus == msgHandleStatus)
                .WhereIF(formType != SettingConfig.AllString, s => s.appname == formType)
                .WhereIF(msgStatus!= SettingConfig.AllString, s=> s.msgstatus == status)
                .OrderBy(s=>s.createtime,OrderByType.Desc)
                .Select("s.*, sc.appnamechs");

            var tttt = queryable.ToSql();
            //queryable.Where("t.msghandlestatus in (@status)", new { status = builderHanderStatus(handleStatus) });

            var data = queryable.ToPageList(pageIndex, pageSize, ref totalCount);

            var existedUnreadCount = queryable.Where(s => s.msgstatus == 0 && s.msghandlestatus== "未完成").Count();

            var t = new BizListResult<SignMessageBox>
            {
                Code= existedUnreadCount,
                total = totalCount,
                Rows = data,
                Msg = "成功"
            };
            return t;
        }


        /// <summary>
        /// 获得未
        /// </summary>
        /// <param name="toempid"></param>
        /// <param name="startD"></param>
        /// <param name="endD"></param>
        /// <returns></returns>
        public BizResult<SignMessageBox> GetUnReadCount(string toempid, DateTime startD, DateTime endD, string formType, string msgstatus)
        {
            var status = msgstatus == "未读" ? 0 : 1;

            var count = db.Queryable<SignMessageBox>("s")
                .Where(s => s.toempid == toempid)
                .Where(s => s.sendtime >= startD && s.sendtime < endD.AddDays(1))
                .WhereIF(formType != SettingConfig.AllString, s => s.appname == formType)
                .Where(s => s.msghandlestatus == "未处理")
               .Where(s => s.msgstatus == 0).Count();


            return new BizResult<SignMessageBox>
            {
                Success = true,
                Msg = count.ToString()
            };
        }
    }
}
