using NexChip.SignMessage.Bussiness.Models.Dtos;
using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Services;
using NexChip.SignMessage.Token;
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
        /// 根据角色OID生成key
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private string getJWTString(SignMessageRole saveEntity)
        {
            TokenModel tm = new TokenModel();
            tm.Role = "Client";
            tm.Project = "SignMessage";
            tm.Uid = saveEntity.OID;

            return JwtHelper.IssueJWT(tm);
        }

        public BizResult<SignMessageRole> Delete(string[] OIDs)
        {
            try
            {
                var res = Service.Dels(OIDs);
                return new BizResult<SignMessageRole>()
                {
                    Success = res
                };
            }
            catch (Exception ex)
            {
                return new BizResult<SignMessageRole>()
                {
                    Success = false,
                    Msg = ex.Message
                };
            }


        }

        private bool insertSignMessageRole(SignMessageRole saveEntity)
        {
            saveEntity.OID = Guid.NewGuid().ToString();
            saveEntity.reservedkey2 = getJWTString(saveEntity);

            saveEntity.creater = "admin";
            saveEntity.createtime = DateTime.Now;

            int i = Service.InsertIgnoreNullColumn(saveEntity);
            return true;
        }

        /// <summary>
        /// 更新特定列
        /// </summary>
        /// <param name="saveEntity"></param>
        /// <returns></returns>
        private bool updateSignMessageRole(SignMessageRole saveEntity)
        {
            saveEntity.updater = "admin";
            saveEntity.updatetime = DateTime.Now;

            int i = Service.UpdateOnlyColumn(saveEntity, it => new { it.updater, it.updatetime, it.appname, it.appnamechs, it.reservedkey1,it.isshow });
            return true;
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
                    OID = dto.OID,
                    appname = dto.appname,
                    appnamechs = dto.appnamechs,
                    reservedkey1 = dto.reservedkey1,
                    isshow=dto.isshow,
                    rolestatus = (int)StatusEnum.Valid
                };

                res.Success = Service.checkAppNameExist(saveEntity);
                if (!res.Success)
                {
                    res.Msg = "名称已存在，重新命名";
                    return res;
                }

                if (string.IsNullOrEmpty(dto.OID))
                {
                    res.Success = insertSignMessageRole(saveEntity);
                }
                else
                {
                    res.Success = updateSignMessageRole(saveEntity);
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
            int start = (reqP.offset - 1) * limit + 1; //前台使用了1开始的页码
            if (string.IsNullOrEmpty(reqP.sortway)) //排序方式为空， 倒序
            {
                ordertype = 1;
            }

            Expression<Func<SignMessageRole, bool>> whereExpression = null;
            Expression<Func<SignMessageRole, object>> orderByExpression = p => p.createtime;

            return Service.GetPageList(start, limit, orderByExpression, ordertype, whereExpression);
        }

        /// <summary>
        /// DataTable 
        /// </summary>
        /// <param name="reqP"></param>
        /// <returns></returns>
        public BizListResultForDataTables<SignMessageRole> ListForDataTables(DataTablesRequsetDto reqP1)
        {
            int ordertype = 0;
            int limit = reqP1.length;
            int start = reqP1.start; 

            if (string.IsNullOrEmpty(reqP1.orderString)) //排序方式为空， 倒序
            {
                ordertype = 1;
            }

            Expression<Func<SignMessageRole, bool>> whereExpression = null;
            Expression<Func<SignMessageRole, object>> orderByExpression = p => new {p.isshow, p.createtime };

            var res =  Service.GetPageList(start, limit, orderByExpression, ordertype, whereExpression);
            return new BizListResultForDataTables<SignMessageRole>
            {
                data = res.Rows,
                draw = reqP1.draw + 1,
                recordsFiltered = res.total,
                recordsTotal = res.total
            };
        }


        public BizResult<SignMessageRole> GetS(string OID)
        {
            Expression<Func<SignMessageRole, bool>> whereExpression = p => p.OID == OID;

            return Service.GetSingle(whereExpression);
        }

    }
}
