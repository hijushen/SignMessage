using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public class LoginUserHelper
    {
        public static string GetLoginUserName(string logonid, string identityName)
        {
            if(logonid == null)
            {
                string[] domains = identityName.Split("\\");
                if(domains.Length > 1)
                {
                    return domains[domains.Length - 1];
                }
                else
                {
                    return domains[0];
                }
            }
            else//切换模式，地址栏自带名称
            {
                return logonid;
            }
        }
    }
}
