using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    public abstract class DbContext
    {
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
        //public DbContext()
        //{
        //    Db = new SqlSugarClient(new ConnectionConfig()
        //    {
        //        ConnectionString = BaseDBConfig.ConnectionString,
        //        DbType = DbType.SqlServer,
        //        IsAutoCloseConnection = true
        //    });
        //}
        //public SqlSugarClient Db;//用来处理事务多表查询和复杂的操作
    }
}
