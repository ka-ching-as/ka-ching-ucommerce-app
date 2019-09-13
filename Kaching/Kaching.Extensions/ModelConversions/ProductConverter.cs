using UCommerce.EntitiesV2;
using Kaching.Extensions.Model;
using Kaching.Extensions.Localization;
using System.Collections.Generic;
using UCommerce.Infrastructure.Logging;
using System.Linq;
using UCommerce.Infrastructure;

namespace Kaching.Extensions.ModelConversions
{
    public class ProductConverter
    {
        private ILoggingService logging;

        public ProductConverter(ILoggingService loggingService)
        {
            logging = loggingService;
        }

        public KachingProduct ConvertProduct(Product subject)
        {
            var product = new KachingProduct();

            AddBasicProperties(subject, product);
            AddAttributes(subject, product);
            AddVariants(subject, product);
            AddTags(subject, product);
            return product;
        }

        private MarketPrice GetMarketPrice(ICollection<ProductPrice> prices)
        {
            if (prices.Count == 0)
            {
                return null;
            }
            else if (prices.Count == 1)
            {
                return new MarketPrice(prices.First().Price.Amount);
            }
            else
            {
                var priceDict = new Dictionary<string, decimal>();
                foreach (var price in prices)
                {
                    priceDict[price.Price.PriceGroup.Name] = price.Price.Amount;
                }
                return new MarketPrice(priceDict);
            }
        }

        private void AddBasicProperties(Product subject, KachingProduct product)
        {
            product.Name = Localizer.GetLocalizedName(new LocalizableProduct(subject));
            product.Description = Localizer.GetLocalizedName(new LocalizableProductLongDescription(subject));
            if (subject.ProductPrices.Count > 0)
            {
                product.RetailPrice = GetMarketPrice(subject.ProductPrices);
            }
            product.Id = subject.Guid.ToString();
            if (!string.IsNullOrEmpty(subject.PrimaryImageMediaId))
            {
                var imageUrl = ObjectFactory.Instance.Resolve<UCommerce.Content.IImageService>().GetImage(subject.PrimaryImageMediaId).Url;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.ImageUrl = imageUrl;
                }
            }
        }

        private void AddTags(Product subject, KachingProduct product)
        {
            product.Tags = new Dictionary<string, bool>();
            product.Tags["ucommerce"] = true;
            if (logging != null)
            {
                logging.Log<ProductConverter>("CATEGORIES");
            }
            // TODO: If no categories exist, then this is the first time the product
            // is saved. This means that we cannot add the correct tags yet.
            // Perhaps skip sync and let this be handled by Category save instead.
            foreach (var category in subject.GetCategories())
            {
                if (logging != null)
                {
                    logging.Log<ProductConverter>(category.Name);
                }

                product.Tags[category.Name] = true;
                var categoryIteration = category;
                while (categoryIteration.ParentCategory != null)
                {
                    categoryIteration = categoryIteration.ParentCategory;
                    product.Tags[categoryIteration.Name] = true;
                }
            }
        }

        private void AddAttributes(Product subject, KachingProduct product)
        {
            if (subject.ProductProperties.Count > 0)
            {
                var attributes = new Dictionary<string, string>();
                foreach (var prop in subject.ProductProperties)
                {
                    var value = prop.Value;
                    var name = prop.ProductDefinitionField.Name;
                    attributes[name] = value;
                }
                product.Attributes = attributes;
            }
        }

        private void AddVariants(Product subject, KachingProduct product)
        {
            var attributePropertyMap = new Dictionary<string, ProductDefinitionField>();
            var list = new List<Variant>();
            foreach (var v in subject.Variants)
            {
                var variant = new Variant();
                AddBasicVariantProperties(v, variant);
                AddVariantAttributes(attributePropertyMap, v, variant);

                list.Add(variant);
            }

            if (list.Count > 0)
            {
                product.Variants = list.ToArray();

                AddDimensions(product, attributePropertyMap, list);
            }
        }

        private void AddDimensions(KachingProduct product, Dictionary<string, ProductDefinitionField> attributePropertyMap, List<Variant> list)
        {
            foreach (var attribute in attributePropertyMap.Keys)
            {
                var flag = false;
                var alreadyUsed = new HashSet<string>();
                foreach (var variant in list)
                {
                    if (variant.Attributes == null || variant.Attributes[attribute] == null)
                    {
                        flag = true;
                        break;
                    }
                    var value = variant.Attributes[attribute];
                    if (alreadyUsed.Contains(value))
                    {
                        flag = true;
                        break;
                    }
                    alreadyUsed.Add(value);
                }
                if (flag)
                {
                    continue;
                }
                else
                {
                    // attribute uniquely identifies each variant - use it for a dimension
                    var propertyDefinition = attributePropertyMap[attribute];
                    var dimension = new Dimension();
                    dimension.Id = propertyDefinition.Name;
                    dimension.Name = Localizer.GetLocalizedName(new LocalizableProductDefinitionField(propertyDefinition));
                    dimension.Values = new List<DimensionValue>();
                    foreach (var property in propertyDefinition.DataType.DataTypeEnums)
                    {
                        var value = new DimensionValue();
                        value.Id = property.Name;
                        value.Name = Localizer.GetLocalizedName(new LocalizableDataTypeEnum(property));
                        dimension.Values.Add(value);
                    }
                    product.Dimensions = new List<Dimension>();
                    product.Dimensions.Add(dimension);
                    foreach (var variant in product.Variants)
                    {
                        variant.DimensionValues = new Dictionary<string, string>();
                        variant.DimensionValues[dimension.Id] = variant.Attributes[attribute];
                    }
                    break;
                }
            }
        }

        private void AddVariantAttributes(Dictionary<string, ProductDefinitionField> attributePropertyMap, Product v, Variant variant)
        {
            if (v.ProductProperties.Count > 0)
            {
                var attributes = new Dictionary<string, string>();
                foreach (var prop in v.ProductProperties)
                {
                    var value = prop.Value;
                    var name = prop.ProductDefinitionField.Name;
                    attributes[name] = value;
                    //attributeSet.Add(name);
                    attributePropertyMap[name] = prop.ProductDefinitionField;
                }
                variant.Attributes = attributes;
            }
        }

        private void AddBasicVariantProperties(Product v, Variant variant)
        {
            variant.Id = v.ProductId.ToString();
            variant.Name = Localizer.GetLocalizedName(new LocalizableProduct(v));
            variant.RetailPrice = GetMarketPrice(v.ProductPrices);
            if (!string.IsNullOrEmpty(v.PrimaryImageMediaId))
            {
                var imageUrl = ObjectFactory.Instance.Resolve<UCommerce.Content.IImageService>().GetImage(v.PrimaryImageMediaId).Url;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    variant.ImageUrl = imageUrl;
                }
            }
        }
    }
}
