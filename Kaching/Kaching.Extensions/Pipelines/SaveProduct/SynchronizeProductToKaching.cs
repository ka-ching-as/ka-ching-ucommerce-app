using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using UCommerce.Infrastructure.Logging;
using Kaching.Extensions.Synchronization;
using Kaching.Extensions.Model;
using Kaching.Extensions.ModelConversions;
using System.Threading.Tasks;
using System;
using Kaching.Extensions.Entities;
using UCommerce.Infrastructure;
using System.Linq;

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
            var config = KachingConfiguration.Get();
            var url = config.ProductsIntegrationURL;
            if (!url.StartsWith("https://", StringComparison.Ordinal))
            {
                return PipelineExecutionResult.Success;
            }

            // In case a product is initially being created, it will not yet have a
            // category configured. In this case delay the synchronization slightly
            // in order to let the database save the product<->category relationship.

            Func<PipelineExecutionResult> synchronize = () =>
            {
                var converter = new ProductConverter(logging);
                var product = converter.ConvertProduct(subject);

                KachingProduct[] products = new KachingProduct[1];
                products[0] = product;
                var metadata = converter.GetMetadata();

                return Synchronizer.Post(new ProductsRequest(products, metadata), url);
            };


            if (subject.GetCategories().Count == 0)
            {
                Task.Delay(2000).ContinueWith((task) =>
                {
                    synchronize();                 
                });
                return PipelineExecutionResult.Success;
            }

            return synchronize();
        }
    }
}
