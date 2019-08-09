using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    public class SignMessgeBiz
    {
        private SignMessageRoleService Service = new SignMessageRoleService();


        public DataTable getTest()
        {
            return Service.TestList();
        }
        
    }
}
