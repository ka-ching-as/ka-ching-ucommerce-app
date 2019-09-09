using System;
using System.Collections.Generic;
using System.Linq;
using UCommerce.EntitiesV2;

namespace Kaching.Extensions.Localization
{
    public class LocalizableCategory : IHasLocalizations
    {
        Category category { get; }
        public LocalizableCategory(Category category)
        {
            this.category = category;
        }
        public ICollection<ILocalized> Localizations
        {
            get
            {
                var list = new List<LocalizedCategoryDescription>();
                foreach (var desc in category.CategoryDescriptions)
                {
                    list.Add(new LocalizedCategoryDescription(desc));
                }
                return list.ToList<ILocalized>();
            }
        }
        public string DefaultName { get { return category.Name; } }
    }

    public class LocalizedCategoryDescription : ILocalized
    {
        CategoryDescription description { get; }
        public LocalizedCategoryDescription(CategoryDescription description)
        {
            this.description = description;
        }
        public string CultureCode { get { return description.CultureCode; } }
        public string DisplayName { get { return description.DisplayName; } }
    }
}
