using NexChip.SignMessage.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NexChip.SignMessage.Services
{
    public class SignMessageRoleService : BaseService<SignMessageRole>
    {
        public DataTable TestList()
        {

            var dt = db.Ado.GetDataTable(@"SELECT  *
FROM    dbo.Student AS a
        LEFT OUTER JOIN dbo.SignMessageRole AS b ON a.Name = b.OID where 1=@id;", new SugarParameter("@id", "1"));

            return dt;
        }


        public bool checkAppNameExist(SignMessageRole saveEntity)
        {
            if (string.IsNullOrEmpty(saveEntity.OID)) //新增的判断重名
            {
                int count = sdb.Count<SignMessageRole>(t => t.appname == saveEntity.appname || t.appnamechs == saveEntity.appnamechs);
                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else //更新判断重名
            {
                var existEntity = sdb.GetSingle<SignMessageRole>(t => t.OID == saveEntity.OID);

                int count = sdb.Count<SignMessageRole>(t => (t.appname == saveEntity.appname || t.appnamechs == saveEntity.appnamechs) 
                && (t.OID != existEntity.OID));
                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        public DataTable getEmployee(string email)
        {
            var dt = db.Ado.GetDataTable(@"SELECT * FROM [NHOADBS02].[HRDATA].[dbo].[HR_EMPLOYEE]
WHERE email = @email;", new SugarParameter("@email", email));

            return dt;
        }


        public List<string> GetDistinctFormNames()
        {
            return db.Queryable<SignMessageRole>().Where(p => p.isshow == 1).Select(p => p.appnamechs).Distinct().ToList();
        }

        public string getAppNameByChs(string appNamechs)
        {
            return db.Queryable<SignMessageRole>().Where(p => p.appnamechs == appNamechs).Select(p => p.appname).Single();
        }
    }
}
