using NexChip.SignMessage.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    public class SignMessgeBiz
    {
        public SimpleClient<SignMessageModel> sdb = new SimpleClient<SignMessageModel>(BaseDB.GetClient());


        public SignMessageModel GetById(long id)
        {
            return sdb.GetById(id);
        }

        
    }
}
