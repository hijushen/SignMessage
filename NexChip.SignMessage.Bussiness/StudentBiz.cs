using NexChip.SignMessage.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NexChip.SignMessage.Bussiness
{
    public class StudentBiz
    {
        //private SqlSugarClient db = BaseDB.GetClient();
        public SimpleClient<Student> sdb = new SimpleClient<Student>(BaseDB.GetClient());

        public TableModel<Student> GetPageList(int pageIndex, int pageSize)
        {
            PageModel p = new PageModel() { PageIndex = pageIndex, PageSize = pageSize };
            Expression<Func<Student, bool>> ex = (it => 1 == 1);
            List<Student> data = sdb.GetPageList(ex, p);
            TableModel<Student> t = new TableModel<Student>();
            t.Code = 0;
            t.Count = p.PageCount;
            t.Data = data;
            t.Msg = "成功";
            return t;
        }

        public Student GetById(long id)
        {
            return sdb.GetById(id);
        }

        private MessageModel<Student> WrapReturnMessage(bool success)
        {
            return new MessageModel<Student> {
                Success = success,
                Msg = success ? "操作成功" : "操作失败"
            };
        }

        public MessageModel<Student> Add(Student entity)
        {
            return WrapReturnMessage(sdb.Insert(entity));
        }

        public MessageModel<Student> Update(Student entity)
        {
            return WrapReturnMessage(sdb.Update(entity));
        }

        public MessageModel<Student> Dels(dynamic[] ids)
        {
            return WrapReturnMessage(sdb.DeleteByIds(ids));
        }
    }
}
