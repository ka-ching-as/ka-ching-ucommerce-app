DROP TABLE Ucommerce_KachingConfiguration;
CREATE TABLE Ucommerce_KachingConfiguration (
  KachingConfigurationId int identity primary key,
  ProductsIntegrationURL nvarchar(max),
  FoldersIntegrationURL nvarchar(max),
  TagsIntegrationURL nvarchar(max)
)