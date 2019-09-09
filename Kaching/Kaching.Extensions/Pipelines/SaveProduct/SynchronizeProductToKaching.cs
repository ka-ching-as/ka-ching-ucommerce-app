using System.Linq;
using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using UCommerce.Infrastructure;
using UCommerce.Infrastructure.Logging;

namespace Kaching.Extensions.Pipelines.SaveProduct
{
    public class SynchronizeProductToKaching : IPipelineTask<Product>
    {
        private ILoggingService logging;

        public SynchronizeProductToKaching(ILoggingService loggingService)
        {
            logging = loggingService;
        }

        public PipelineExecutionResult Execute(Product subject)
        {
            var product = new KachingProduct();

            product.Name = Localizer.GetLocalizedName(new LocalizableProduct(subject));
            product.Description = Localizer.GetLocalizedName(new LocalizableProductLongDescription(subject));
          
            product.RetailPrice = subject.ProductPrices.First().Price.Amount;
            product.Id = subject.Guid.ToString();

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

            var attributePropertyMap = new Dictionary<string, ProductDefinitionField>();
            //var attributeSet = new HashSet<string>();
            var list = new List<Variant>();
            foreach (var v in subject.Variants)
            {
                var variant = new Variant();
                variant.Id = v.ProductId.ToString();
                variant.Name = Localizer.GetLocalizedName(new LocalizableProduct(v));

                if (v.ProductPrices.Count > 0)
                {
                    variant.RetailPrice = v.ProductPrices.First().Price.Amount;
                }
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

                    if (!string.IsNullOrEmpty(v.PrimaryImageMediaId))
                    {
                        var imageUrl = ObjectFactory.Instance.Resolve<UCommerce.Content.IImageService>().GetImage(v.PrimaryImageMediaId).Url;
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            variant.ImageUrl = imageUrl;
                        }
                    }

                }

                list.Add(variant);
            }

            if (list.Count > 0)
            {
                product.Variants = list.ToArray();

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
                    } else
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

            if (!string.IsNullOrEmpty(subject.PrimaryImageMediaId))
            {
                var imageUrl = ObjectFactory.Instance.Resolve<UCommerce.Content.IImageService>().GetImage(subject.PrimaryImageMediaId).Url;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    product.ImageUrl = imageUrl;
                }
            }

            product.Tags = new Dictionary<string, bool>();
            product.Tags["ucommerce"] = true;

            logging.Log<SynchronizeProductToKaching>("CATEGORIES");
            // TODO: If no categories exist, then this is the first time the product
            // is saved. This means that we cannot add the correct tags yet.
            // Perhaps skip sync and let this be handled by Category save instead.
            foreach (var category in subject.GetCategories())
            {
                logging.Log<SynchronizeProductToKaching>(category.Name);
                product.Tags[category.Name] = true;
                var categoryIteration = category;
                while (categoryIteration.ParentCategory != null)
                {
                    categoryIteration = categoryIteration.ParentCategory;
                    product.Tags[categoryIteration.Name] = true;
                }
            }

            var url = "REDACTED";


            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";

            Stream dataStream = request.GetRequestStream();

            ProductsRequest productsRequest = new ProductsRequest();

            productsRequest.Products = new Dictionary<string, KachingProduct>();

            productsRequest.Products.Add(product.Id, product);
            
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            string jsonProducts = JsonConvert.SerializeObject(productsRequest, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(jsonProducts);
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();

            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
                return PipelineExecutionResult.Success;

            return PipelineExecutionResult.Error;
        }
    }

    public class ProductsRequest
    {
        public Dictionary<string, KachingProduct> Products { get; set; }
    }
}
