using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UCommerce.Pipelines;

namespace Kaching.Extensions.Synchronization
{
    public class Synchronizer
    {
        private static PipelineExecutionResult Send<T>(T value, string url, string method)
        {

            WebRequest request = WebRequest.Create(url);

            request.Method = method;
            request.ContentType = "application/json";

            Stream dataStream = request.GetRequestStream();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string jsonProducts = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonProducts);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                return PipelineExecutionResult.Error;

            return PipelineExecutionResult.Success;
        }

        public static PipelineExecutionResult Post<T>(T value, string url)
        {
            return Send(value, url, "POST");
        }

        public static PipelineExecutionResult Delete<T>(T value, string url)
        {
            return Send(value, url, "DELETE");
        }
    }
}
