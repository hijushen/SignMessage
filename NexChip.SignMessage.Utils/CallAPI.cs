using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexChip.SignMessage.Utils
{
    public class CallAPI
    {
        public IRestResponse Execute(RestRequest request) 
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(SettingConfig.PostUrl);

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var twilioException = new ApplicationException(message, response.ErrorException);
                throw twilioException;
            }
            return response;
        }

        // TwilioApi.cs, method of TwilioApi class
        public IRestResponse PostCall(string url, string json) 
        {
            var request = new RestRequest(url, Method.POST);
            request.Timeout = 1000 * 15;
            // easily add HTTP Headers
            request.AddHeader("Authorization", string.Format("{0}", SettingConfig.ApiTokenString));
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            request.AddParameter("cache-control", "no-cache", ParameterType.HttpHeader);
            request.RequestFormat = DataFormat.Json;


            return Execute(request);
        }
    }
}
