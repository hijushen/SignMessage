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

        public BizListResult<Student> GetPageList(int pageIndex, int pageSize)
        {
            return Service.GetPageList(pageIndex, pageSize);
        }

        public BizResult<Student> Add(Student entity)
        {
            if (Service.Add(entity))
                return new BizResult<Student> { Success = true, Msg = "操作成功" };
            else
                return new BizResult<Student> { Success = false, Msg = "操作失败" };
        }

        public BizResult<Student> Update(Student entity)
        {
            if (Service.Update(entity))
                return new BizResult<Student> { Success = true, Msg = "操作成功" };
            else
                return new BizResult<Student> { Success = false, Msg = "操作失败" };
        }

        public BizResult<Student> Dels(dynamic[] ids)
        {
            if (Service.Dels(ids))
                return new BizResult<Student> { Success = true, Msg = "操作成功" };
            else
                return new BizResult<Student> { Success = false, Msg = "操作失败" };
        }
    }
}
