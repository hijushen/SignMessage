using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public static class NewOARestSharpHttp
    {
        public static readonly RestClient restClient;

        static NewOARestSharpHttp()
        {
            restClient = new RestClient(new Uri(DNSHelper.getRemoteIPUrlPortPath(SettingConfig.NewOAUrl)));
        }

        /// <summary>
        /// 使用post方法请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public static string PostJson(string url,Object data)
        {
            //url = "/Values/AccessEform";
            var baseUrlStr = DNSHelper.getRemoteIPUrlPortPath(SettingConfig.NewOAUrl);
            if (restClient.BaseUrl != new Uri(baseUrlStr))
            {
                restClient.BaseUrl = new Uri(baseUrlStr);
            }

            var request = new RestRequest(url, Method.POST);
            request.Timeout = 1000 * 10;

            // easily add HTTP Headers
            //request.AddHeader("User-Agent", string.Format("{0}", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.132 Safari/537.36"));

            request.AddParameter("application/json; charset=utf-8", data.SerializeModel(), ParameterType.RequestBody);
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
                return "";
            }

        }

        public static string HttpPostJson(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = System.Text.Encoding.GetEncoding("utf-8");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                posturl = SettingConfig.NewOAUrl + posturl;
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                //request.Headers.Add(HttpRequestHeader.Authorization, string.Format("{0}", SettingConfig.AppTokenString));
                //request.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
                //request.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                //request.Headers.Add(HttpRequestHeader.Accept, "application/json");                

                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                request.ContentLength = data.Length;

                using (outstream = request.GetRequestStream())
                {
                    outstream.Write(data, 0, data.Length);
                    //outstream.Close();
                }

                try
                {
                    response = request.GetResponse() as HttpWebResponse;
                    instream = response.GetResponseStream();

                    //发送请求并获取相应回应数据
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    sr = new StreamReader(instream, encoding);
                    //返回结果网页（html）代码
                    string content = sr.ReadToEnd();
                    string err = string.Empty;
                    return content;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
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
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;


            //// execute the request
            IRestResponse response = restClient.Execute(request);
            var content = response.Content; // raw content as string
            return content;

        }
    }
}
