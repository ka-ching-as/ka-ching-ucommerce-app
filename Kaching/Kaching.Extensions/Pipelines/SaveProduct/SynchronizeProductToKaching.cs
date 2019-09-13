using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using UCommerce.Infrastructure.Logging;
using Kaching.Extensions.Synchronization;
using Kaching.Extensions.Model;
using Kaching.Extensions.ModelConversions;

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
            var converter = new ProductConverter(logging);
            var product = converter.ConvertProduct(subject);

            var url = "REDACTED";

            KachingProduct[] products = new KachingProduct[1];
            products[0] = product;

            return Synchronizer.Post(new ProductsRequest(products), url);
        }
    }
}
