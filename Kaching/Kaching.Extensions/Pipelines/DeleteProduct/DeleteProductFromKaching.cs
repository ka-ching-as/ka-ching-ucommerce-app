using System.Collections.Generic;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure.Logging;
using UCommerce.Pipelines;
using Kaching.Extensions.Synchronization;

namespace Kaching.Extensions.Pipelines.DeleteProduct
{
    public class DeleteProductFromKaching : IPipelineTask<Product>
    {
        private ILoggingService logging;

        public DeleteProductFromKaching(ILoggingService loggingService)
        {
            logging = loggingService;
        }

        public PipelineExecutionResult Execute(Product subject)
        {
            var idToDelete = subject.Guid.ToString();

            logging.Log<DeleteProductFromKaching>("Deleting product: " + subject.Name + " in Ka-ching");
            var url = "https://us-central1-ka-ching-base-dev.cloudfunctions.net/imports/products?account=brugsen&apikey=ABC&integration=ucommerce";

            var idsToDelete = new List<string>();
            idsToDelete.Add(idToDelete);
            var deletionRequest = new ProductDeletionRequest();
            deletionRequest.Ids = idsToDelete;
            return Synchronizer.Delete(deletionRequest, url);
        }
    }

    public class ProductDeletionRequest
    {
        public List<string> Ids { get; set; }
    }
}
