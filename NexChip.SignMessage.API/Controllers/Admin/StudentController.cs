using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexChip.SignMessage.Bussiness;
using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Token;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NexChip.SignMessage.API.Controllers.Admin
{

    /// <summary>
    /// 学生模块
    /// </summary>
    [Produces("application/json")]
    [Route("api/admin/[controller]")]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class StudentController : Controller
    {
        private StudentBiz bll = new StudentBiz();
        private SignMessgeBiz sbiz = new SignMessgeBiz();
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

        /// <summary>
        /// TestSome
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("TestSome")]
        [Authorize(Roles ="User,Admin")]
        public JsonResult TestSome()
        {
            return Json(sbiz.getTest());
        }

        [HttpGet]
        [Route("GetJWTStr")]
        public JsonResult GetJWTStr(string id="admin", string sub="Admin",string project="signmessage")
        {
            TokenModel tm = new TokenModel()
            {
                Uid = id,
                Role = sub,
                Project=project
            };

            string jswStr = JwtHelper.IssueJWT(tm);
            return Json(jswStr);
        }
        #endregion

    }
}
