using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using System.Collections.Generic;
using UCommerce.Infrastructure.Logging;
using Kaching.Extensions.Localization;
using Kaching.Extensions.Synchronization;
using Kaching.Extensions.Model;

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
            var tag = new KachingTag();
            tag.Tag = subject.Name;
            tag.Name = Localizer.GetLocalizedName(new LocalizableCategory(subject));

            logging.Log<SynchronizeCategoryToKaching>(subject.Name);
            logging.Log<SynchronizeCategoryToKaching>(subject.ModifiedOn.ToString());
            logging.Log<SynchronizeCategoryToKaching>("Products");

            foreach (var product in subject.Products)
            {
                // TODO: For all 'new' products, sync product to Ka-ching...
                // Alternatively - sync all products in category always??
                logging.Log<SynchronizeCategoryToKaching>(product.Name);
                logging.Log<SynchronizeCategoryToKaching>(product.CreatedOn.ToString());
                logging.Log<SynchronizeCategoryToKaching>(product.ModifiedOn.ToString());
            }

            PipelineExecutionResult result = PostTag(tag);
            if (result == PipelineExecutionResult.Error)
                return PipelineExecutionResult.Error;

            return UpdateFolders(subject);
        }

        private static PipelineExecutionResult PostTag(KachingTag tag)
        {
            var url = "REDACTED";

            var tags = new List<KachingTag>();
            tags.Add(tag);

            return Synchronizer.Post(tags, url);
        }

        private static Folder GenerateFolderTree(Category category)
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

        public static PipelineExecutionResult UpdateFolders(Category subject)
        {
            var folders = new List<Folder>();
            foreach (var category in subject.ProductCatalog.GetRootCategories())
            {
                var folder = GenerateFolderTree(category);
                folders.Add(folder);
            }

            return PostFolders(folders);
        }

        private static PipelineExecutionResult PostFolders(List<Folder> folders)
        {
            var url = "REDACTED";
            return Synchronizer.Post(folders, url);
        }
    }
}
