using System;
using System.Collections.Generic;
using Kaching.Extensions.Entities;
using Kaching.Extensions.ModelConversions;
using Kaching.Extensions.Synchronization;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure.Logging;
using UCommerce.Pipelines;

namespace Kaching.Extensions.Pipelines.DeleteCategory
{
    public class DeleteCategoryFromKaching : IPipelineTask<Category>
    {
        private ILoggingService logging;

        public DeleteCategoryFromKaching(ILoggingService loggingService)
        {
            logging = loggingService;
        }

        public PipelineExecutionResult Execute(Category subject)
        {
            var idToDelete = subject.Name;

            logging.Log<DeleteCategoryFromKaching>("Deleting tag: " + subject.Name + " in Ka-ching");
            var config = KachingConfiguration.Get();
            var url = config.TagsIntegrationURL;
            if (url.StartsWith("https://", StringComparison.Ordinal))
            {
                var idsToDelete = new List<string>();
                idsToDelete.Add(idToDelete);
                var result = Synchronizer.Delete(idsToDelete, url);
                if (result == PipelineExecutionResult.Error)
                    return result;
            }


            var folders = new CategoryConverter(logging).GetFolders();
            var foldersUrl = config.FoldersIntegrationURL;
            if (!foldersUrl.StartsWith("https://", StringComparison.Ordinal))
            {
                return PipelineExecutionResult.Success;
            }
            return Synchronizer.Post(folders, foldersUrl);
        }
    }
}
