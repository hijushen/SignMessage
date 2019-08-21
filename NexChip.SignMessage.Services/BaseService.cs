using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NexChip.SignMessage.Services
{
    /// <summary>
    /// 服务层基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseService<T> where T : class, new()
    {
        public BaseService()
        {
            db = GetClient();
            sdb = db.GetSimpleClient();
        }
        public SqlSugarClient db;
        public SimpleClient sdb;
        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns></returns>
        private SqlSugarClient GetClient()
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
                Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
            return db;
        }

        #region CRUD
        public BizListResult<T> GetPageList(int start, int pageSize, 
            Expression<Func<T, object>> orderByExpression = null, 
            int ordertype=0,
            Expression<Func<T, bool>> whereExpression = null)
        {
            OrderByType orderByType = OrderByType.Asc;
            if (ordertype == 1)
            {
                orderByType = OrderByType.Desc;
            }

            if (whereExpression == null)
            {
                whereExpression = it => 1 == 1;
            }

            double page = start / pageSize ;
            int pageIndex = Math.Ceiling(page).ObjToInt() + 1;//Sql Sugar 页从1开始！
            PageModel p = new PageModel() { PageIndex = pageIndex, PageSize = pageSize };
          
            List<T> data = sdb.GetPageList(whereExpression, p, orderByExpression, orderByType);
            var t = new BizListResult<T>
            {
                Code = 0,
                total = p.PageCount,
                Rows = data,
                Msg = "成功"
            };
            return t;
        }

        public T Get(long id)
        {
            return sdb.GetById<T>(id);
        }

        public BizResult<T> GetSingle(Expression<Func<T, bool>> whereExpression)
        {
            var res =  sdb.GetSingle<T>(whereExpression);
            if (res == null) return null;

            return new BizResult<T>
            {
                Data = res,
                Msg = "成功",
                Success = true
            };
        }

        public bool Insert(T entity)
        {
            return sdb.Insert(entity);
        }

        public int InsertIgnoreNullColumn(T entiy)
        {
            return db.Insertable<T>(entiy).IgnoreColumns(ignoreNullColumn: true).ExecuteCommand();
        }

        public bool Update(T entity)
        {
            return sdb.Update(entity);
        }

        public bool Dels(dynamic[] ids)
        {
            return sdb.DeleteByIds<T>(ids);
        }
        #endregion
    }
}
