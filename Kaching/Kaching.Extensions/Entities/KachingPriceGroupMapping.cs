using System;
using UCommerce.EntitiesV2;
using UCommerce.EntitiesV2.Maps;

namespace Kaching.Extensions.Entities
{
    public class KachingPriceGroupMapping : IEntity
    {
        public virtual int KachingPriceGroupMappingId { get; protected set; }

        public virtual PriceGroup PriceGroup { get; set; }
        public virtual string MarketCode { get; set; }

        public virtual int Id { get { return KachingPriceGroupMappingId; } }
    }

    public class KachingPriceGroupMappingMap : BaseClassMap<KachingPriceGroupMapping>
    {
        public KachingPriceGroupMappingMap()
        {
            //Table used to store data
            Table("Ucommerce_KachingPriceGroupMapping");

            // The primary key
            Id(x => x.KachingPriceGroupMappingId);

            References(x => x.PriceGroup);
            Map(x => x.MarketCode);
        }
    }

}
