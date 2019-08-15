using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NexChip.SignMessage.Bussiness
{
    public class StudentBiz
    {
        private StudentService Service = new Services.StudentService();

        public Student GetById(long id)
        {
            return Service.Get(id);
        }

        public TableModel<Student> GetPageList(int pageIndex, int pageSize)
        {
            return Service.GetPageList(pageIndex, pageSize);
        }

        public MessageModel<Student> Add(Student entity)
        {
            if (Service.Add(entity))
                return new MessageModel<Student> { Success = true, Msg = "操作成功" };
            else
                return new MessageModel<Student> { Success = false, Msg = "操作失败" };
        }

        public MessageModel<Student> Update(Student entity)
        {
            if (Service.Update(entity))
                return new MessageModel<Student> { Success = true, Msg = "操作成功" };
            else
                return new MessageModel<Student> { Success = false, Msg = "操作失败" };
        }

        public MessageModel<Student> Dels(dynamic[] ids)
        {
            if (Service.Dels(ids))
                return new MessageModel<Student> { Success = true, Msg = "操作成功" };
            else
                return new MessageModel<Student> { Success = false, Msg = "操作失败" };
        }
    }
}
