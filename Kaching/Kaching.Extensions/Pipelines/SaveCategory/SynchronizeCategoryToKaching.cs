using System;
using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using UCommerce.Infrastructure;
using System.Net;
using System.IO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kaching.Extensions.Pipelines.SaveCategory
{
    public class SynchronizeCategoryToKaching : IPipelineTask<Category>
    {
        public Folder GenerateFolderTree(Category category)
        {
            var folder = new Folder(category.Name);
            var childCategories = category.GetCategories();
            if (childCategories == null || childCategories.Count == 0)
            {
                return folder;
            }

            var children = new List<Folder>();
            foreach (var childCategory in childCategories)
            {
                var childFolder = GenerateFolderTree(childCategory);
                children.Add(childFolder);
            }
            folder.Children = children;
            return folder;
        }


        public PipelineExecutionResult Execute(Category subject)
        {
            var tag = new KachingTag();
            tag.Tag = subject.Name;
            tag.Name = new L10nString(subject.Name);

            WebResponse response = PostTag(tag);
            if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                return PipelineExecutionResult.Error;

            var folders = new List<Folder>();
            foreach (var category in subject.ProductCatalog.GetRootCategories())
            {
                var folder = GenerateFolderTree(category);
                folders.Add(folder);
            }

            response = PostFolders(folders);
            if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                return PipelineExecutionResult.Error;

            return PipelineExecutionResult.Success;
        }

        private static WebResponse PostTag(KachingTag tag)
        {
            var url = "REDACTED";


            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";

            Stream dataStream = request.GetRequestStream();

            var tags = new List<KachingTag>();
            tags.Add(tag);

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string jsonProducts = JsonConvert.SerializeObject(tags, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonProducts);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            return response;
        }

        private static WebResponse PostFolders(List<Folder> folders)
        {
            var url = "REDACTED";

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";

            Stream dataStream = request.GetRequestStream();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string jsonProducts = JsonConvert.SerializeObject(folders, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonProducts);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            return response;
        }
    }
}
