using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public static class DNSHelper
    {
        /// <summary>
        ///转换配置的服务地址
        /// </summary>
        /// <param name="RemoteHostName"></param>
        /// <returns></returns>
        public static string getRemoteIPUrlPortPath(string RemoteHostName)
        {
            //如下测试通过， 主机或IP返回 IP端口路径方式
            //string RemoteHostName1 = "http://e10101j00682.chn.nexchip:9000/api";
            //string RemoteHostName2 = "http://10.50.64.30:9000/api";
            //string RemoteHostName3 = "http://10.50.6.95/MessageSignBox.API/api";
            //string RemoteHostName4 = "http://nhoaapt01.chn.nexchip/MessageSignBox.API/api";

            //返回2，3形式


            //var s1 = getRemoteIPUrlPortPath(RemoteHostName1);
            //var s2 = getRemoteIPUrlPortPath(RemoteHostName2);
            //var s3 = getRemoteIPUrlPortPath(RemoteHostName3);
            //var s4 = getRemoteIPUrlPortPath(RemoteHostName4);

            RemoteHostName = RemoteHostName.Substring("http://".Length);
            string[] arrayRemoteHostName = RemoteHostName.Split("/");

            string ipv4OptionPort = "";
            string[] arrayHostNamePort = arrayRemoteHostName[0].Split(":");
            try
            {
                IPHostEntry ipEntry = Dns.GetHostEntry(arrayHostNamePort[0]);

                foreach (var item in ipEntry.AddressList)
                {
                    if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipv4OptionPort = item.ToString();
                    }
                };

                if (arrayHostNamePort.Length == 2)
                {
                    ipv4OptionPort = ipv4OptionPort + ":" + arrayHostNamePort[1];
                }


                string returnRes = ipv4OptionPort;
                for (int i = 1; i < arrayRemoteHostName.Length; i++)
                {
                    returnRes = returnRes + "/" + arrayRemoteHostName[i];
                }

                return "http://" + returnRes;
            }
            catch (Exception)
            {
                return "";
                throw new Exception("解析配置地址失败");
            }
        }
    }
}
