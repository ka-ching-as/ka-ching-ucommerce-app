using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using System.Collections.Generic;
using UCommerce.Infrastructure.Logging;
using Kaching.Extensions.Localization;
using Kaching.Extensions.Synchronization;
using Kaching.Extensions.Model;
using Kaching.Extensions.ModelConversions;

namespace Kaching.Extensions.Pipelines.SaveCategory
{
    public class SynchronizeCategoryToKaching : IPipelineTask<Category>
    {
        private ILoggingService logging;

        public SynchronizeCategoryToKaching(ILoggingService loggingService)
        {
            logging = loggingService;
        }

        public PipelineExecutionResult Execute(Category subject)
        {
            var tag = new CategoryConverter(logging).ConvertToTag(subject);

            logging.Log<SynchronizeCategoryToKaching>(subject.Name);
            logging.Log<SynchronizeCategoryToKaching>(subject.ModifiedOn.ToString());

            PipelineExecutionResult result = PostTag(tag);
            if (result == PipelineExecutionResult.Error)
                return PipelineExecutionResult.Error;

            return UpdateFolders(subject);
        }

        private PipelineExecutionResult PostTag(KachingTag tag)
        {
            var url = "REDACTED";

            var tags = new List<KachingTag>();
            tags.Add(tag);

            return Synchronizer.Post(tags, url);
        }

        public PipelineExecutionResult UpdateFolders(Category subject)
        {
            var folders = new CategoryConverter(logging).GetFolders();
            var url = "REDACTED";
            return Synchronizer.Post(folders, url);
        }
    }
}
