using NexChip.SignMessage.Bussiness.Models.Dtos;
using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    public class SignMessageRoleBiz
    {
        private SignMessageRoleService Service = new SignMessageRoleService();


        public BizResult<SignMessageRoleDto> register(SignMessageRoleDto dto, ClaimsPrincipal User)
        {
            try
            {
                SignMessageRole entity = new SignMessageRole
                {
                    OID = Guid.NewGuid().ToString(),
                    appname = dto.appname,
                    creater = "admin",
                    updater = "admin",
                    createtime = DateTime.Now,
                    updatetime = DateTime.Now
                };

                var isSuccess = Service.Insert(entity);
                return new BizResult<SignMessageRoleDto>
                {
                    Success = isSuccess
                };
            }
            catch (Exception ex)
            {
                return new BizResult<SignMessageRoleDto>()
                {
                    Success = false,
                    Msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BizResult<SignMessageRole> EditSave(SignMessageRoleDto dto)
        {
            var res = new BizResult<SignMessageRole>
            {
                Success = false
            };
            
            if (dto == null) return res;

            try
            {
                SignMessageRole saveEntity = new SignMessageRole()
                {
                    appname = dto.appname,
                    reservedkey1 = dto.reservedkey1
                };

                if (string.IsNullOrEmpty(dto.OID))
                {
                    saveEntity.OID = Guid.NewGuid().ToString();
                    saveEntity.creater = "admin";
                    saveEntity.createtime = DateTime.Now;
                    var saveRes = Service.InsertIgnoreNullColumn(saveEntity);

                    res.Success = true;
                }
                else
                {
                    saveEntity.updater = "admin";
                    saveEntity.updatetime = DateTime.Now;

                    res.Success = Service.Update(saveEntity);
                }


                res.Data = saveEntity;
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }

            return res;
        }

        /// <summary>
        /// int offset, int limit
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public BizListResult<SignMessageRole> List(SignMessageRoleDto reqP)
        {
            int ordertype = 0;
            int limit = reqP.limit;
            int start = (reqP.offset - 1)*limit + 1; //前台使用了1开始的页码
            if (string.IsNullOrEmpty(reqP.sortway)) //排序方式为空， 倒序
            {
                ordertype = 1;
            }

            Expression<Func<SignMessageRole, bool>> whereExpression = null;
            Expression<Func<SignMessageRole, object>> orderByExpression = p => p.createtime;

            return Service.GetPageList(start, limit, orderByExpression, ordertype, whereExpression);
        }

        public BizResult<SignMessageRole> GetS(string OID)
        {
            Expression<Func<SignMessageRole, bool>> whereExpression = p => p.OID == OID;

            return Service.GetSingle(whereExpression);
        }

    }
}
