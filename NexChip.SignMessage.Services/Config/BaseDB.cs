﻿using NexChip.SignMessage.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexChip.SignMessage.Services
{
    public class BaseDB
    {
        public static SqlSugarClient GetClient()
        {
            SqlSugarClient db = new SqlSugarClient(
                new ConnectionConfig()
                {
                    ConnectionString = BaseDBConfig.ConnectionString,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true
                }
            );
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                LogHelper.Info("fdfd");
                Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
            return db;
        }
    }
}
