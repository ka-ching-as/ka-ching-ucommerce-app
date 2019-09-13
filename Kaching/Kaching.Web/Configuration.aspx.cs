using System;
using System.Collections.Generic;
using Kaching.Extensions.Model;
using Kaching.Extensions.ModelConversions;
using Kaching.Extensions.Synchronization;
using UCommerce.Api;
using UCommerce.EntitiesV2;

namespace Kaching.Web
{
	public partial class Configuration : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

        private int ImportCategories(ICollection<Category> categories)
        {
            var productCount = 0;
            foreach (var category in categories)
            {
                var products = category.Products;
                productCount += ImportProducts(products);
                var childCategories = category.Categories;
                productCount += ImportCategories(childCategories);
            }
            return productCount;
        }

        private int ImportProducts(IEnumerable<Product> products)
        {
            var converter = new ProductConverter(null);
            List<KachingProduct> kachingProducts = new List<KachingProduct>();
            foreach (var product in products)
            {
                kachingProducts.Add(converter.ConvertProduct(product));
            }

            var url = "REDACTED";

            Synchronizer.Post(new ProductsRequest(kachingProducts.ToArray()), url);
            return kachingProducts.Count;
        }

        protected void StartImportButton_Click(Object sender, EventArgs e)
        {
            var categories = CatalogLibrary.GetRootCategories();
            var count = ImportCategories(categories);
            ImportStatus.Text = $"Initiated import of {count} products.";
        }
    }
}
