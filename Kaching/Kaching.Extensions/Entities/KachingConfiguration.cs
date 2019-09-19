using System;
using System.Linq;
using UCommerce.EntitiesV2;
using UCommerce.EntitiesV2.Maps;
using UCommerce.Infrastructure;

namespace Kaching.Extensions.Entities
{
    public class KachingConfiguration : IEntity
    {
        public static KachingConfiguration Get()
        {
            var repository = ObjectFactory.Instance.Resolve<IRepository<KachingConfiguration>>();
            var configs = repository.Select();

            var config = configs.FirstOrDefault();
            if (config == null)
            {
                config = new KachingConfiguration();
            }
            return config;
        }

        public virtual int KachingConfigurationId { get; protected set; }

        public virtual string ProductsIntegrationURL { get; set; }
        public virtual string FoldersIntegrationURL { get; set; }
        public virtual string TagsIntegrationURL { get; set; }

        public virtual int Id { get { return KachingConfigurationId; } }
    }

    public class KachingConfigurationMap : BaseClassMap<KachingConfiguration>
    {
        public KachingConfigurationMap()
        {
            //Table used to store data
            Table("Ucommerce_KachingConfiguration");

            // The primary key
            Id(x => x.KachingConfigurationId);

            // Our simple field, FluentNHibernate knows
            // how they go together because the field and
            // column are named the same.
            Map(x => x.ProductsIntegrationURL);
            Map(x => x.FoldersIntegrationURL);
            Map(x => x.TagsIntegrationURL);
        }
    }

    public class NHibernateMappingsTag : IContainsNHibernateMappingsTag
    {

    }
}

