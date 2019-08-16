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

        public BizResult<SignMessageBox> MockMessageBox()
        {
            try
            {
                Service.MockMessages();

                return new BizResult<SignMessageBox>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new BizResult<SignMessageBox>
                {
                    Success = false,
                    Msg=ex.Message
                };
            }
        }

    }
}
