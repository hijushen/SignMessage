using NexChip.SignMessage.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Services
{
    public class TempService : BaseService<SignMessageBox>
    {

        ///生成测试用的签核箱数据
        ///

        public void MockMessages()
        {
            List<SignMessageBox> mockData = new List<SignMessageBox>();

            for (int i = 0; i < 10; i++)
            {
                mockData.Add(new SignMessageBox()
                {
                    OID = Guid.NewGuid().ToString(),
                    appname = "test",
                    msgsourceid = Guid.NewGuid().ToString(),
                    msgstatus =0,
                    emergencylevel=0,
                    sendtime=DateTime.Now,
                    fromempid="E00000",
                    fromempname="TEST00-N",
                    toempid="TEST01",
                    toempname="TEST01-N",
                    callbackurl="http://www.baidu.com",
                    substance="MockData"+i,
                    showmsg="Substance，Data"+i,
                    createtime=DateTime.Now,
                    updatetime=DateTime.Now,
                    remark="MockData"+i
                });
            }

            var s = db.Insertable<SignMessageBox>(mockData).ToSql();
            //插入

            db.Insertable<SignMessageBox>(mockData).ExecuteCommand();
        }
    }
}
