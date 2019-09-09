using System;
using System.Collections.Generic;
using System.Linq;
using UCommerce.EntitiesV2;

namespace Kaching.Extensions
{
    public class LocalizableProductDefinitionField : IHasLocalizations
    {
        ProductDefinitionField field { get; }
        public LocalizableProductDefinitionField(ProductDefinitionField field)
        {
            this.field = field;
        }
        public ICollection<ILocalized> Localizations
        {
            get
            {
                var list = new List<LocalizedProductDefinitionFieldDescription>();
                foreach (var desc in field.ProductDefinitionFieldDescriptions)
                {
                    list.Add(new LocalizedProductDefinitionFieldDescription(desc));
                }
                return list.ToList<ILocalized>();
            }
        }
        public string DefaultName { get { return field.Name; } }
    }

    public class LocalizedProductDefinitionFieldDescription : ILocalized
    {
        ProductDefinitionFieldDescription description { get; }
        public LocalizedProductDefinitionFieldDescription(ProductDefinitionFieldDescription description)
        {
            this.description = description;
        }
        public string CultureCode { get { return description.CultureCode; } }
        public string DisplayName { get { return description.DisplayName; } }
    }
}
