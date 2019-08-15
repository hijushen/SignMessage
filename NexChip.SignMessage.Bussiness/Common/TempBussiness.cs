using NexChip.SignMessage.Entities;
using NexChip.SignMessage.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Bussiness
{
    public class TempBussiness
    {
        private TempService Service = new TempService();

        public MessageModel<SignMessageBox> MockMessageBox()
        {
            try
            {
                Service.MockMessages();

                return new MessageModel<SignMessageBox>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new MessageModel<SignMessageBox>
                {
                    Success = false,
                    Msg=ex.Message
                };
            }
        }

    }
}
