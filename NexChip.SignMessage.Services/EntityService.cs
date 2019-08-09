﻿using NexChip.SignMessage.IServices;
using NexChip.SignMessage.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Services
{
    public class EntityService: BaseDB, IEntity 
    {
        public SqlSugarClient db = GetClient();
        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool CreateEntity(string entityName, string filePath)
        {
            try
            {
                db.DbFirst.IsCreateAttribute().Where(entityName).CreateClassFile(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
