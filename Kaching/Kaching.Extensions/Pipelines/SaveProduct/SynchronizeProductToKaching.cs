using UCommerce.EntitiesV2;
using UCommerce.Pipelines;
using UCommerce.Infrastructure.Logging;
using Kaching.Extensions.Synchronization;
using Kaching.Extensions.Model;
using Kaching.Extensions.ModelConversions;
using System.Threading.Tasks;
using System;

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
            // In case a product is initially being created, it will not yet have a
            // category configured. In this case delay the synchronization slightly
            // in order to let the database save the product<->category relationship.

            Func<PipelineExecutionResult> synchronize = () =>
            {
                var converter = new ProductConverter(logging);
                var product = converter.ConvertProduct(subject);

                var url = "REDACTED";

                KachingProduct[] products = new KachingProduct[1];
                products[0] = product;

                return Synchronizer.Post(new ProductsRequest(products), url);
            };


            if (subject.GetCategories().Count == 0)
            {
                logging.Log<SynchronizeProductToKaching>("NO CATEGORIES - SLEEP");
                Task.Delay(2000).ContinueWith((task) =>
                {
                    logging.Log<SynchronizeProductToKaching>("NO CATEGORIES - DONE SLEEPING! COUNT: " + subject.GetCategories().Count);
                    synchronize();                 
                });
                return PipelineExecutionResult.Success;
            }

            return synchronize();
        }
    }
}
