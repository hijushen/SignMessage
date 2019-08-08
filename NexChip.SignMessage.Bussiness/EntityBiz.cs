
using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Model;
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
        //public SqlSugarClient db = GetClient();
        public SqlSugarClient Db => GetInstance();

        private SqlSugarClient GetInstance()
        {
            var Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = BaseDBConfig.ConnectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true
            });

            return Db;
        }
        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public MessageModel<string> CreateEntity(string entityName, string contentRootPath)
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
                    var s = Db.Ado.GetDataTable("select * from Student");
                    //var s2 = Db.Queryable<Student>().ToSql();
                    //Db.DbFirst.IsCreateAttribute().Where(entityName).CreateClassFile(filePath);
                    return new MessageModel<string> { Success = true, Msg = "生成成功" };

                }
                catch(Exception ex)
                {
                    return new MessageModel<string> { Success = false, Msg = ex.Message };
                }
                
                //db.DbFirst.IsCreateAttribute().Where(entityName).CreateClassFile(filePath);
                //return true;
            }
            catch (Exception)
            {
                return new MessageModel<string> { Success=false};
            }
        }
    }
}
