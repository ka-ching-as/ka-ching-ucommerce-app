using System;
using System.Collections.Generic;
using System.Linq;
using UCommerce.EntitiesV2;

namespace Kaching.Extensions
{
    public class LocalizableDataTypeEnum : IHasLocalizations
    {
        DataTypeEnum dataTypeEnum { get; }
        public LocalizableDataTypeEnum(DataTypeEnum dataTypeEnum)
        {
            this.dataTypeEnum = dataTypeEnum;
        }
        public ICollection<ILocalized> Localizations
        {
            get
            {
                var list = new List<LocalizedDataTypeEnumDescription>();
                foreach (var desc in dataTypeEnum.DataTypeEnumDescriptions)
                {
                    list.Add(new LocalizedDataTypeEnumDescription(desc));
                }
                return list.ToList<ILocalized>();
            }
        }
        public string DefaultName { get { return dataTypeEnum.Name; } }
    }

    public class LocalizedDataTypeEnumDescription : ILocalized
    {
        DataTypeEnumDescription description { get; }
        public LocalizedDataTypeEnumDescription(DataTypeEnumDescription description)
        {
            this.description = description;
        }
        public string CultureCode { get { return description.CultureCode; } }
        public string DisplayName { get { return description.DisplayName; } }
    }
}
