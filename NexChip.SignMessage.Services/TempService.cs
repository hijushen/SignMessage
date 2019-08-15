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
                    fromempname="模拟工号1",
                    toempid="E00001",
                    toempname="模拟工号2",
                    callbackurl="http://www.baidu.com",
                    substance="模拟数据"+i,
                    showmsg="标题，数据"+i,
                    createtime=DateTime.Now,
                    updatetime=DateTime.Now,
                    remark="测试数据"+i
                });
            }

            var s = db.Insertable<SignMessageBox>(mockData).ToSql();
           //插入
           db.Insertable<SignMessageBox>(mockData).ExecuteCommand();
        }
    }
}
