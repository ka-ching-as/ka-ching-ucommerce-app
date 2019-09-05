# How to Setup the Sample App

The uCommerce sample app has multiple configuration options and ways to control what parts of the app is shown/enabled.


## Configure the UIs

### Tab

The tab is the UI with the most configuration options in sample app, below is the configuration of tab's configuration object that controls what is displayed on the tab and whether to display the tab or not.

{CODE-START:xml /}
<configuration>
	<components>
		<!-- Shows how you can control the value of properties on an object through castle windsor config -->
		<component
				id="SampleApp.TabConfiguration"
				service="SampleApp.Extensions.Configuration.TabConfiguration, SampleApp.Extensions"
				type="SampleApp.Extensions.Configuration.TabConfiguration, SampleApp.Extensions">
			<parameters>
				<ShowTab>true</ShowTab>
				<ShowUCommerceVersion>true</ShowUCommerceVersion>
				<ShowShemaVersion>true</ShowShemaVersion>
			</parameters>
		</component>
	</components>
</configuration>
{CODE-END /}

The tab configuration object has three options that you can configure, the first is whether to show the tab or not.
The second option is whether you want the tab to display the version of uCommerce that is installed and the last option is whether the tabs should display the database's schema version.

Another way to disable the tab is to find the tab.config file and add ".disabled" this will let uCommerce know that you don't want use the config file, the tab.config can be found under the website at uCommerce/Apps/SampleApp/Configuration.

### Subpage

This is a subpage.
