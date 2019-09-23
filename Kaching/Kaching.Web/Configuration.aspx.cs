using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
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

        private void UpdateButtonState()
        {
            var config = KachingConfiguration.Get();
            var urls = new List<string> { config.ProductsIntegrationURL, config.TagsIntegrationURL, config.FoldersIntegrationURL };
            var canImport = urls.All(s => s != null && s.StartsWith("https:", StringComparison.Ordinal));

            StartCategoryImportButton.Enabled = canImport;
            StartProductImportButton.Enabled = canImport;
        }

        protected void CultureCodeList_EditCommand(object source, DataListCommandEventArgs e)
        {
            // Set the DataList's EditItemIndex property to the
            // index of the DataListItem that was clicked
            CultureCodeList.EditItemIndex = e.Item.ItemIndex;
            // Rebind the data to the DataList
            BindCultureCodeList();
        }

        protected void CultureCodeList_CancelCommand(object source, DataListCommandEventArgs e)
        {
            // Set the DataList's EditItemIndex property to -1
            CultureCodeList.EditItemIndex = -1;
            // Rebind the data to the DataList
            BindCultureCodeList();
        }

        protected void CultureCodeList_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            //int countryId = Convert.ToInt32(CultureCodeList.DataKeys[e.Item.ItemIndex]);
            //var countryRepository = ObjectFactory.Instance.Resolve<IRepository<Country>>();
            //var countries = countryRepository.Select();
            var row = cultureCodeData.Rows[e.Item.ItemIndex];
            var cultureCode = row.Field<string>(1);
            var mappingRepository = ObjectFactory.Instance.Resolve<IRepository<KachingCultureCodeMapping>>();
            var mappings = mappingRepository.Select();
            var mapping = mappings.FirstOrDefault(m => m.CultureCode == cultureCode);
            if (mapping != null)
            {
                mappingRepository.Delete(mapping);
            }

            // Set the DataList's EditItemIndex property to -1
            CultureCodeList.EditItemIndex = -1;
            GetCultureCodeData();
            BindCultureCodeList();
        }

        protected void CultureCodeList_UpdateCommand(object source, DataListCommandEventArgs e)
        {
            var row = cultureCodeData.Rows[e.Item.ItemIndex];
            var cultureCode = row.Field<string>(1);
            //int countryId = Convert.ToInt32(PriceGroupList.DataKeys[e.Item.ItemIndex]);
            TextBox languageCodeField = (TextBox)e.Item.FindControl("LanguageCodeField");

            var countryRepository = ObjectFactory.Instance.Resolve<IRepository<Country>>();
            var countries = countryRepository.Select();

            var mappingRepository = ObjectFactory.Instance.Resolve<IRepository<KachingCultureCodeMapping>>();
            var mappings = mappingRepository.Select();
            var mapping = mappings.FirstOrDefault(m => m.CultureCode == cultureCode);
            if (mapping == null)
            {
                mapping = new KachingCultureCodeMapping();
                mapping.CultureCode = cultureCode;
            }
            mapping.LanguageCode = languageCodeField.Text;

            mappingRepository.Save(mapping);
            CultureCodeList.EditItemIndex = -1;
            GetCultureCodeData();
            BindCultureCodeList();
        }

        private DataTable cultureCodeData;

        private void BindCultureCodeList()
        {
            CultureCodeList.DataSource = cultureCodeData;
            CultureCodeList.DataBind();
        }

        private void GetCultureCodeData()
        {
            var cultureCodeRepository = ObjectFactory.Instance.Resolve<IRepository<Country>>();
            var mappingRepository = ObjectFactory.Instance.Resolve<IRepository<KachingCultureCodeMapping>>();
            var mappings = mappingRepository.Select();
            var countries = cultureCodeRepository.Select();

            cultureCodeData = new DataTable();

            cultureCodeData.Columns.Add("CountryId");
            cultureCodeData.Columns.Add("CultureCode");
            cultureCodeData.Columns.Add("LanguageCode");

            foreach (var country in countries)
            {
                // The implementation of the 'default' is exactly dropping everything from the first dash.
                var language = country.Culture.Split('-').First();
                var firstMapping = mappings.FirstOrDefault(m => m.CultureCode == country.Culture);
                if (firstMapping != null)
                {
                    language = firstMapping.LanguageCode;
                }
                cultureCodeData.Rows.Add(country.Id, country.Culture, language);
            }
        }


        // Price group

        protected void PriceGroupList_EditCommand(object source, DataListCommandEventArgs e)
        {
            // Set the DataList's EditItemIndex property to the
            // index of the DataListItem that was clicked
            PriceGroupList.EditItemIndex = e.Item.ItemIndex;
            // Rebind the data to the DataList
            BindPriceGroupList();
        }

        protected void PriceGroupList_CancelCommand(object source, DataListCommandEventArgs e)
        {
            // Set the DataList's EditItemIndex property to -1
            PriceGroupList.EditItemIndex = -1;
            // Rebind the data to the DataList
            BindPriceGroupList();
        }

        protected void PriceGroupList_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            int priceGroupId = Convert.ToInt32(PriceGroupList.DataKeys[e.Item.ItemIndex]);
            var mappingRepository = ObjectFactory.Instance.Resolve<IRepository<KachingPriceGroupMapping>>();
            var mappings = mappingRepository.Select();
            var mapping = mappings.FirstOrDefault(m => m.PriceGroup.PriceGroupId == priceGroupId);
            if (mapping != null)
            {
                mappingRepository.Delete(mapping);
            }

            // Set the DataList's EditItemIndex property to -1
            PriceGroupList.EditItemIndex = -1;
            GetPriceGroupData();
            BindPriceGroupList();
        }

        protected void PriceGroupList_UpdateCommand(object source, DataListCommandEventArgs e)
        {
            int priceGroupId = Convert.ToInt32(PriceGroupList.DataKeys[e.Item.ItemIndex]);
            TextBox marketName = (TextBox)e.Item.FindControl("MarketField");

            var priceGroupRepository = ObjectFactory.Instance.Resolve<IRepository<PriceGroup>>();
            var priceGroups = priceGroupRepository.Select();

            var mappingRepository = ObjectFactory.Instance.Resolve<IRepository<KachingPriceGroupMapping>>();
            var mappings = mappingRepository.Select();
            var mapping = mappings.FirstOrDefault(m => m.PriceGroup.PriceGroupId == priceGroupId);
            if (mapping == null)
            {
                mapping = new KachingPriceGroupMapping();
                mapping.PriceGroup = priceGroups.FirstOrDefault(p => p.PriceGroupId == priceGroupId);
            }
            mapping.MarketCode = marketName.Text;

            if (mapping.PriceGroup != null)
            {
                mappingRepository.Save(mapping);
            }
            PriceGroupList.EditItemIndex = -1;
            GetPriceGroupData();
            BindPriceGroupList();
        }

        private DataTable priceGroupData;

        private void BindPriceGroupList()
        {
            PriceGroupList.DataSource = priceGroupData;
            PriceGroupList.DataBind();
        }


        private void GetPriceGroupData()
        {
            var priceGroupRepository = ObjectFactory.Instance.Resolve<IRepository<PriceGroup>>();
            var mappingRepository = ObjectFactory.Instance.Resolve<IRepository<KachingPriceGroupMapping>>();
            var mappings = mappingRepository.Select();
            var priceGroups = priceGroupRepository.Select();

            priceGroupData = new DataTable();

            priceGroupData.Columns.Add("PriceGroupId");
            priceGroupData.Columns.Add("PriceGroup");
            priceGroupData.Columns.Add("Market");

            foreach (var priceGroup in priceGroups)
            {
                var market = priceGroup.Name;
                var firstMapping = mappings.FirstOrDefault(m => m.PriceGroup == priceGroup);
                if (firstMapping != null)
                {
                    market = firstMapping.MarketCode;
                }
                priceGroupData.Rows.Add(priceGroup.Id, priceGroup.Name, market);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
		{
            UpdateButtonState();
            GetPriceGroupData();
            GetCultureCodeData();

            if (!Page.IsPostBack)
            {
                BindPriceGroupList();
                BindCultureCodeList();

                var config = KachingConfiguration.Get();

                ProductsImportURL.Text = config.ProductsIntegrationURL;
                TagsImportURL.Text = config.TagsIntegrationURL;
                FoldersImportURL.Text = config.FoldersIntegrationURL;
            }
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

            var metadata = converter.GetMetadata();

            Synchronizer.Post(new ProductsRequest(kachingProducts.ToArray(), metadata), url);
            return kachingProducts.Count;
        }

        protected void StartProductImportButton_Click(Object sender, EventArgs e)
        {
            var productRepository = ObjectFactory.Instance.Resolve<IRepository<Product>>();
            var products = productRepository.Select();

            var batchList = new List<Product>();
            var batchSize = 100;
            var count = 0;
            var total = 0;
            foreach (var product in products)
            {
                if (product.IsVariant) { continue; }
                batchList.Add(product);
                if (count >= batchSize)
                {
                    count = 0;
                    ImportProducts(batchList);
                    batchList = new List<Product>();
                }
                count++;
                total++;
            }

            if (count >= 0)
            {
                ImportProducts(batchList);
            }

            ImportStatus.Text = $"Initiated import of {total} products.";
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

            UpdateButtonState();
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
