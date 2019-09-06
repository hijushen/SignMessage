using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public static class RestSharpHttp
    {
        public static readonly RestClient restClient;

        static RestSharpHttp()
        {
            restClient = new RestClient(new Uri(getRemoteIPUrlPortPath(SettingConfig.PostUrl)));
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public static string PostJson1(string url, string json)
        {

            var request = new RestRequest(url, Method.POST);
            request.Timeout = 1000 * 15;
            // easily add HTTP Headers
            request.AddHeader("Authorization", string.Format("{0}", SettingConfig.ApiTokenString));
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            //try
            //{
            //    restClient.ExecuteAsync(request, response =>
            //    {
            //        if (response.StatusCode == HttpStatusCode.OK)
            //        {
            //            // OK
            //        }
            //        else
            //        {
            //        }
            //    });
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

            //// execute the request
            IRestResponse response = restClient.Execute(request);
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                var content = response.Content; // raw content as string
                return content;
            }
            else
            {
                LogHelper.Error(response.StatusDescription, new Exception { Source = response.StatusDescription });
                return "";
            }

            //return content;
            //// or automatically deserialize result
            //// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            //RestResponse<Person> response2 = client.Execute<Person>(request);
            //var name = response2.Data.Name;


        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public static string PostJson(string url, string json)
        {
            var baseUrlStr = getRemoteIPUrlPortPath(SettingConfig.PostUrl);
            if (restClient.BaseUrl != new Uri(baseUrlStr))
            {
                restClient.BaseUrl = new Uri(baseUrlStr);
            }

            var request = new RestRequest(url, Method.POST);
            request.Timeout = 1000 * 15;

            // easily add HTTP Headers
            request.AddHeader("Authorization", string.Format("{0}", SettingConfig.ApiTokenString));
            //request.AddHeader("User-Agent", string.Format("{0}", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36"));

            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.AddParameter("cache-control", "no-cache", ParameterType.HttpHeader);
            request.RequestFormat = DataFormat.Json;

            //restClient.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
            restClient.MaxRedirects = 3;
            //restClient.Timeout = 1000 * 15;

            //// execute the request
            IRestResponse response = restClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content; // raw content as string
                return content;
            }
            else
            {
                LogHelper.Error(response.StatusCode.ToString(), new Exception { Source = response.StatusDescription });
                return "";
            }

            //return content;
            //// or automatically deserialize result
            //// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            //RestResponse<Person> response2 = client.Execute<Person>(request);
            //var name = response2.Data.Name;


        }




        /// <summary>
        /// 使用Get方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public static string GetJson(string url, string json)
        {
            var request = new RestRequest(url, Method.GET);
            request.Timeout = 1000 * 15;
            // easily add HTTP Headers
            request.AddHeader("Authorization", string.Format("{0}", SettingConfig.ApiTokenString));
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;


            //// execute the request
            IRestResponse response = restClient.Execute(request);
            var content = response.Content; // raw content as string
            return content;

        }



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
            catch(Exception)
            {
                return "";
                throw new Exception("解析配置地址失败");
            }
        }

    }
}
