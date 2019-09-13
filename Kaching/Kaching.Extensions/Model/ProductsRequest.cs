using System;
namespace Kaching.Extensions.Model
{
    public class ProductsRequest
    {
        public KachingProduct[] Products { get; set; }

        public ProductsRequest(KachingProduct[] products)
        {
            Products = products;
        }
    }
}
