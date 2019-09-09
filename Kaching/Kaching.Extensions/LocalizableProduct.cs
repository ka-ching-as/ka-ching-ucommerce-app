using System;
using System.Collections.Generic;
using System.Linq;
using UCommerce.EntitiesV2;

namespace Kaching.Extensions
{
    public class LocalizableProduct : IHasLocalizations
    {
        Product product{ get; }
        public LocalizableProduct(Product product)
        {
            this.product = product;
        }
        public ICollection<ILocalized> Localizations
        {
            get
            {
                var list = new List<LocalizedProductName>();
                foreach (var desc in product.ProductDescriptions)
                {
                    list.Add(new LocalizedProductName(desc));
                }
                return list.ToList<ILocalized>();
            }
        }
        public string DefaultName { get { return product.Name; } }
    }

    public class LocalizedProductName : ILocalized
    {
        ProductDescription description { get; }
        public LocalizedProductName(ProductDescription description)
        {
            this.description = description;
        }
        public string CultureCode { get { return description.CultureCode; } }
        public string DisplayName { get { return description.DisplayName; } }
    }

    public class LocalizableProductLongDescription : IHasLocalizations
    {
        Product product { get; }
        public LocalizableProductLongDescription(Product product)
        {
            this.product = product;
        }
        public ICollection<ILocalized> Localizations
        {
            get
            {
                var list = new List<LocalizedProductLongDescription>();
                foreach (var desc in product.ProductDescriptions)
                {
                    list.Add(new LocalizedProductLongDescription(desc));
                }
                return list.ToList<ILocalized>();
            }
        }
        public string DefaultName { get { return product.Name; } }
    }

    public class LocalizedProductLongDescription : ILocalized
    {
        ProductDescription description { get; }
        public LocalizedProductLongDescription(ProductDescription description)
        {
            this.description = description;
        }
        public string CultureCode { get { return description.CultureCode; } }
        public string DisplayName { get { return description.LongDescription; } }
    }
}
