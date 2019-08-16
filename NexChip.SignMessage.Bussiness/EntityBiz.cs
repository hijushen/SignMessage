
using NexChip.SignMessage.Entities;
 
 
using NexChip.SignMessage.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    /// <summary>
    /// 实体操作服务
    /// </summary>
    public class EntityBiz
    {
        private EntityService iService = new EntityService();
        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public BizResult<string> CreateEntity(string entityName, string contentRootPath)
        {
            try
            {
                string[] arr = contentRootPath.Split('\\');
                string baseFileProvider = "";
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    baseFileProvider += arr[i];
                    baseFileProvider += "\\";
                }
                string filePath = baseFileProvider + "NexChip.SignMessage.Entities";
                try
                {

                    if (iService.CreateEntity(entityName, filePath))
                        return new BizResult<string> { Success = true, Msg = "生成成功" };
                    else
                        return new BizResult<string> { Success = false, Msg = "生成失败" };


                    //var s = Db.Ado.GetDataTable("select * from Student");
                    //var s2 = Db.Queryable<Student>().ToSql();
                    //Db.DbFirst.IsCreateAttribute().Where(entityName).CreateClassFile(filePath);
                    //return new BizResult<string> { Success = true, Msg = "生成成功" };

                }
                catch(Exception ex)
                {
                    return new BizResult<string> { Success = false, Msg = ex.Message };
                }
                
                //db.DbFirst.IsCreateAttribute().Where(entityName).CreateClassFile(filePath);
                //return true;
            }
            catch (Exception)
            {
                return new BizResult<string> { Success=false};
            }
        }
    }
}
