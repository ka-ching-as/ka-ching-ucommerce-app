using System;
using System.Collections.Generic;
using Kaching.Extensions.Pipelines.SaveCategory;
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
            var url = "REDACTED";

            var idsToDelete = new List<string>();
            idsToDelete.Add(idToDelete);
            var result = Synchronizer.Delete(idsToDelete, url);
            if (result == PipelineExecutionResult.Error)
                return result;

            return SynchronizeCategoryToKaching.UpdateFolders(subject);
        }
    }
}
