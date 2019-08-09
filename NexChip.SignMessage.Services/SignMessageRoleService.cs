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
    }
}
