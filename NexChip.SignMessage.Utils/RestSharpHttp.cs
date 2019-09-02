﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public static class RestSharpHttp
    {
        public static readonly RestClient restClient;

        static RestSharpHttp()
        {
            restClient = new RestClient(new Uri(SettingConfig.PostUrl));
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public static string  PostJson(string url, string json)
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
            var content = response.Content; // raw content as string
            return content;
            //return content;
            //// or automatically deserialize result
            //// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            //RestResponse<Person> response2 = client.Execute<Person>(request);
            //var name = response2.Data.Name;


        }

        // client.Authenticator = new HttpBasicAuthenticator(username, password);

    }
}