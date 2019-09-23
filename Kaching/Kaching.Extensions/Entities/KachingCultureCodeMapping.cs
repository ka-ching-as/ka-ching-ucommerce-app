using System;
using UCommerce.EntitiesV2;
using UCommerce.EntitiesV2.Maps;

namespace Kaching.Extensions.Entities
{
    public class KachingCultureCodeMapping : IEntity
    {
        public virtual int KachingCultureCodeMappingId { get; protected set; }

        public virtual string CultureCode { get; set; }
        public virtual string LanguageCode { get; set; }

        public virtual int Id { get { return KachingCultureCodeMappingId; } }
    }

    public class KachingCultureCodeMappingMap : BaseClassMap<KachingCultureCodeMapping>
    {
        public KachingCultureCodeMappingMap()
        {
            //Table used to store data
            Table("Ucommerce_KachingCultureCodeMapping");

            // The primary key
            Id(x => x.KachingCultureCodeMappingId);

            Map(x => x.CultureCode);
            Map(x => x.LanguageCode);
        }
    }

}
