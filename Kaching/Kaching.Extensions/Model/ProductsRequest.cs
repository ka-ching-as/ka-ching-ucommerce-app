using System;
namespace Kaching.Extensions.Model
{
    public class ProductsRequest
    {
        public KachingProduct[] Products { get; set; }
        public KachingMetadata Metadata { get; set; }

        public ProductsRequest(KachingProduct[] products, KachingMetadata metadata)
        {
            Products = products;
            Metadata = metadata;
        }
    }
}
