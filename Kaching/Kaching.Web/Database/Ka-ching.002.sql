DROP TABLE Ucommerce_KachingCultureCodeMapping;
CREATE TABLE Ucommerce_KachingCultureCodeMapping (
  KachingCultureCodeMappingId int identity primary key,
  CultureCode nvarchar(60),
  LanguageCode nvarchar(60)
)

DROP TABLE Ucommerce_KachingPriceGroupMapping;
CREATE TABLE Ucommerce_KachingPriceGroupMapping (
  KachingPriceGroupMappingId int identity primary key,
  PriceGroupId int not null foreign key references uCommerce_PriceGroup(PriceGroupId),
  MarketCode nvarchar(60)
)