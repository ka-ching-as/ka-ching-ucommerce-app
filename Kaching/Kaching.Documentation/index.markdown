# The Ka-ching app for Ucommerce

## Functionality provided by the Ka-ching app

The Ka-ching app for Ucommerce adds automatic syncronization of Products and Categories from Ucommerce to Products, Tags and Folder hierarchies in Ka-ching.
The app can be configured to provide mappings of Ucommerce Price Groups to Ka-ching Markets as well as Ucommerce Culture Codes to Ka-ching Language codes.
For even more customization, you can freely edit the source code to provide custom logic.

## Data handled by the Ka-ching app

### Products

* Display name for all defined languages
* Description for all defined languages
* Prices for all price groups - mapped to markets in Ka-ching
* Product image if present
* Product properties - mapped to attributes in Ka-ching
* Category hierarchy locations - mapped to product tags in Ka-ching

### Variants

* Display name for all defined languages
* Prices for all price groups - mapped to markets in Ka-ching
* Variant image if present
* Variant properties - mapped to attributes and dimensions in Ka-ching

### Categories
* Categories are mapped to tags - display name is mapped for all defined languages
* Category hierarch is mapped to Folder Structure in Ka-ching

## Handled pipelines

* SaveProduct
* DeleteProduct
* SaveCategory
* DeleteCategory

## Installing the Ka-ching app

The app can be installed as a .nupkg through the Ucommerce App Installation page.

## Configuring the Ka-ching app

After installation, the Ka-ching app adds a configuration page under the Ucommerce Settings.
Here you can configure import endpoints from Ka-ching. You can generate import endpoints by logging into the [Ka-ching Back Office](https://backoffice.ka-ching.dk), enable 'Advanced Options' under the profile button in the top right corner, and then follow the instructions on the Import Integrations page.
The required import endpoints are:

* Products
* Folders
* Tags

After generating the endpoints, the URLs of the endpoints can be copy pasted into the Ka-ching configuration page in Ucommerce.

In case you use multiple Price Groups and you have localized your data in Ucommerce, then you may optionally provide mappings of these in the configuration page.

After configuring the system, you can start initial imports of products and categories, and from there on any changes saved to products and categories will automatically sync to Ka-ching.

## Customizing the Ka-ching app

The source code of the Ka-ching app is completely open, so you are welcome to perform any modifications to the conversion of entities from Ucommerce to Ka-ching.
The namespace Kaching.Extensions.Model contains entities and JSON conversions that match the models expected by Ka-ching. The Kaching.Extensions.Synchronization handles the actual serialization and POST'ing to the Ka-ching import endpoints, to most likely this will not require any changes.
Good candidates for changing are the classes in the Kaching.Extensions.ModelConversions namespace. 

These deal with the actual conversions and here you may for instance choose how custom Data Types map to Product Attributes in Ka-ching.
