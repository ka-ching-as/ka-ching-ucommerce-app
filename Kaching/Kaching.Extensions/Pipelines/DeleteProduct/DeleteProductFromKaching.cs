using System.Collections.Generic;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure.Logging;
using UCommerce.Pipelines;
using Kaching.Extensions.Synchronization;
using Kaching.Extensions.Entities;
using System;

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
            var config = KachingConfiguration.Get();
            var url = config.ProductsIntegrationURL;
            if (!url.StartsWith("https://", StringComparison.Ordinal))
            {
                return PipelineExecutionResult.Success;
            }

            var idToDelete = subject.Guid.ToString();

            logging.Log<DeleteProductFromKaching>("Deleting product: " + subject.Name + " in Ka-ching");
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
