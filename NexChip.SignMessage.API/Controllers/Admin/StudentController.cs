using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexChip.SignMessage.API.Controllers.Admin
{

    /// <summary>
    /// 学生模块
    /// </summary>
    [Produces("application/json")]
    [Route("api/admin/[controller]")]
    public class StudentController : Controller
    {
        private StudentBiz bll = new StudentBiz();
        #region base
        /// <summary>
        /// 获取学生分页列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetStudentPageList(int pageIndex = 1, int pageSize = 10)
        {
            return Json(bll.GetPageList(pageIndex, pageSize));
        }
        /// <summary>
        /// 获取单个学生
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public JsonResult GetStudentById(long id)
        {
            return Json(bll.GetById(id));
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add(Student entity = null)
        {
            if (entity == null)
                return Json("参数为空");
            return Json(bll.Add(entity));
        }

        /// <summary>
        /// 编辑学生
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Student")]
        public JsonResult Update(Student entity = null)
        {
            if (entity == null)
                return Json("参数为空");
            return Json(bll.Update(entity));
        }

        ///// <summary>
        ///// 删除学生
        ///// </summary>
        ///// <param name="ids"></param>GetById
        ///// <returns></returns>
        //[HttpDelete]
        //public JsonResult Dels(dynamic[] ids = null)
        //{
        //    if (ids.Length == 0)
        //        return Json("参数为空");
        //    return Json(bll.Dels(ids));
        //}
        #endregion
    }
}
