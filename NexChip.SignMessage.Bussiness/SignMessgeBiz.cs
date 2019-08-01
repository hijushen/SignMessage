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

        private MessageModel<SignMessageModel> WrapReturnMessage(bool success)
        {
            return new MessageModel<SignMessageModel>
            {
                Success = success,
                Msg = success ? "操作成功" : "操作失败"
            };
        }

        private MessageModel<SignMessageModel> WrapReturnTestMessage(bool success)
        {
            return new MessageModel<SignMessageModel>
            {
                Success = success,
                Msg = success ? "测试模拟调用成功" : "测试模拟调用失败",
                Data=new List<SignMessageModel>()
                {
                    new SignMessageModel
                    {
                        appname="模拟业务平台1",
                        sendtime=DateTime.Now,
                        msgbody = new SignMessageMessageBodyViewModel
                        {
                            showmsg="hello",
                            createtime=DateTime.Now
                        }
                    }
                }
            };
        }

        public MessageModel<SignMessageModel> Add(SignMessageModel entity)
        {
            return WrapReturnMessage(sdb.Insert(entity));
        }

        public MessageModel<SignMessageModel> Update(SignMessageModel entity)
        {
            return WrapReturnMessage(sdb.Update(entity));
        }

        public MessageModel<SignMessageModel> Dels(dynamic[] ids)
        {
            return WrapReturnMessage(sdb.DeleteByIds(ids));
        }

        public object PostUpdateMsg(SignMessageModel entity)
        {
            //return WrapReturnMessage(sdb.Update(entity));
            return WrapReturnTestMessage(true);
        }

        public object PostNewSignMsg(SignMessageModel entity)
        {
            return WrapReturnTestMessage(false);

        }
    }
}
