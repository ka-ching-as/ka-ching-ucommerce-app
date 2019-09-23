using UCommerce.EntitiesV2;
using Kaching.Extensions.Model;
using Kaching.Extensions.Localization;
using System.Collections.Generic;
using UCommerce.Infrastructure.Logging;
using System.Linq;
using UCommerce.Infrastructure;
using Kaching.Extensions.Entities;

namespace Kaching.Extensions.ModelConversions
{
    public class ProductConverter
    {
        private ILoggingService logging;
        private IQueryable<PriceGroup> priceGroups;
        private IQueryable<KachingPriceGroupMapping> priceGroupMappings;

        public ProductConverter(ILoggingService loggingService)
        {
            logging = loggingService;
            var priceGroupRepository = ObjectFactory.Instance.Resolve<IRepository<PriceGroup>>();
            priceGroups = priceGroupRepository.Select();

            var mappingRepository = ObjectFactory.Instance.Resolve<IRepository<KachingPriceGroupMapping>>();
            priceGroupMappings = mappingRepository.Select();
        }

        public KachingMetadata GetMetadata()
        {
            KachingMetadata metadata = null;
            if (priceGroups.Count() > 1)
            {
                metadata = new KachingMetadata();
                foreach (var priceGroup in priceGroups)
                {
                    var market = priceGroup.Name;
                    var mapping = priceGroupMappings.FirstOrDefault(p => p.PriceGroup == priceGroup);
                    if (mapping != null)
                    {
                        market = mapping.MarketCode;
                    }
                    metadata.Markets.Add(market);
                }
            }
            return metadata;
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
            else if (priceGroups.Count() == 1)
            {
                return new MarketPrice(prices.First().Price.Amount);
            }
            else
            {
                var priceDict = new Dictionary<string, decimal>();
                foreach (var price in prices)
                {
                    var key = price.Price.PriceGroup.Name;
                    var mapping = priceGroupMappings.FirstOrDefault(p => p.PriceGroup == price.Price.PriceGroup);
                    if (mapping != null)
                    {
                        key = mapping.MarketCode;
                    }
                    priceDict[key] = price.Price.Amount;
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
            var tags = new HashSet<string>();
            // Just as a convenience, always add a 'ucommerce' tag to allow for easy identification of
            // products from Ucommerce.
            tags.Add("ucommerce");

            foreach (var category in subject.GetCategories())
            {
                tags.Add(category.Name);

                var categoryIteration = category;
                while (categoryIteration.ParentCategory != null)
                {
                    categoryIteration = categoryIteration.ParentCategory;
                    tags.Add(categoryIteration.Name);
                }
            }
            product.Tags = tags;
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
