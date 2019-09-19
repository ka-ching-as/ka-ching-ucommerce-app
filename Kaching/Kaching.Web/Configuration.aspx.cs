using System;
using System.Collections.Generic;
using System.Linq;
using Kaching.Extensions.Entities;
using Kaching.Extensions.Model;
using Kaching.Extensions.ModelConversions;
using Kaching.Extensions.Synchronization;
using UCommerce.Api;
using UCommerce.EntitiesV2;
using UCommerce.Infrastructure;
using UCommerce.Infrastructure.Logging;

namespace Kaching.Web
{
	public partial class Configuration : System.Web.UI.Page
	{
        private ILoggingService logger;

        public Configuration()
        {
            logger = ObjectFactory.Instance.Resolve<ILoggingService>();
        }

        protected void Page_Load(object sender, EventArgs e)
		{
            if (Page.IsPostBack)
            {
                return;
            }

            var config = KachingConfiguration.Get();
            ProductsImportURL.Text = config.ProductsIntegrationURL;
            TagsImportURL.Text = config.TagsIntegrationURL;
            FoldersImportURL.Text = config.FoldersIntegrationURL;
        }

        private int ImportCategoryProducts(ICollection<Category> categories)
        {
            var productCount = 0;
            foreach (var category in categories)
            {
                var products = category.Products;
                productCount += ImportProducts(products);
                var childCategories = category.Categories;
                productCount += ImportCategoryProducts(childCategories);
            }
            return productCount;
        }

        private int ImportProducts(IEnumerable<Product> products)
        {
            var config = KachingConfiguration.Get();
            var url = config.ProductsIntegrationURL;
            if (!url.StartsWith("https://", StringComparison.Ordinal))
            {
                return 0;
            }

            var converter = new ProductConverter(logger);
            List<KachingProduct> kachingProducts = new List<KachingProduct>();
            foreach (var product in products)
            {
                kachingProducts.Add(converter.ConvertProduct(product));
            }

            Synchronizer.Post(new ProductsRequest(kachingProducts.ToArray()), url);
            return kachingProducts.Count;
        }

        protected void StartProductImportButton_Click(Object sender, EventArgs e)
        {
            var categories = CatalogLibrary.GetRootCategories();
            var count = ImportCategoryProducts(categories);
            ImportStatus.Text = $"Initiated import of {count} products.";
        }

        private void PostTags(List<KachingTag> tags)
        {
            var config = KachingConfiguration.Get();
            var url = config.TagsIntegrationURL;
            if (!url.StartsWith("https://", StringComparison.Ordinal))
            {
                return;
            }
            Synchronizer.Post(tags, url);
        }

        public void PostFolders(List<Folder> folders)
        {
            var config = KachingConfiguration.Get();
            var url = config.FoldersIntegrationURL;
            if (!url.StartsWith("https://", StringComparison.Ordinal))
            {
                return;
            }
            Synchronizer.Post(folders, url);
        }

        protected void SaveConfigurationButton_Click(Object sender, EventArgs e)
        {
            var repository = ObjectFactory.Instance.Resolve<IRepository<KachingConfiguration>>();
            var config = KachingConfiguration.Get();
            config.ProductsIntegrationURL = ProductsImportURL.Text;
            config.FoldersIntegrationURL = FoldersImportURL.Text;
            config.TagsIntegrationURL = TagsImportURL.Text;
            repository.Save(config);
        }

        protected void StartCategoryImportButton_Click(Object sender, EventArgs e)
        {
            var converter = new CategoryConverter(logger);
            var folders = converter.GetFolders();
            var tags = converter.GetAllTags();

            PostFolders(folders);
            PostTags(tags);

            var count = tags.Count;
            ImportStatus.Text = $"Initiated import of {count} categories.";
        }

    }
}
