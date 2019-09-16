using System;
using System.Collections.Generic;
using Kaching.Extensions.Localization;
using Kaching.Extensions.Model;
using UCommerce.Api;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure.Logging;

namespace Kaching.Extensions.ModelConversions
{
    public class CategoryConverter
    {
        private ILoggingService logging;

        public CategoryConverter(ILoggingService loggingService)
        {
            logging = loggingService;
        }

        private Folder GenerateFolderTree(Category category)
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

        public List<Folder> GetFolders()
        {
            var folders = new List<Folder>();
            foreach (var category in CatalogLibrary.GetRootCategories())
            {
                var folder = GenerateFolderTree(category);
                folders.Add(folder);
            }

            return folders;
        }

        public KachingTag ConvertToTag(Category category)
        {
            var tag = new KachingTag();
            tag.Tag = category.Name;
            tag.Name = Localizer.GetLocalizedName(new LocalizableCategory(category));
            return tag;
        }

        private List<KachingTag> GetTagsFromCategories(ICollection<Category> categories)
        {
            var tags = new List<KachingTag>();
            foreach (var category in categories)
            {
                var tag = ConvertToTag(category);
                tags.Add(tag);
                if (category.GetCategories().Count > 0)
                {
                    var childTags = GetTagsFromCategories(category.GetCategories());
                    tags.AddRange(childTags);
                }
            }
            return tags;
        }

        public List<KachingTag> GetAllTags()
        {
            return GetTagsFromCategories(CatalogLibrary.GetRootCategories());
        }
    }
}
