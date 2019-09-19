

using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using UCommerce.Infrastructure.Logging;
using UCommerce.Infrastructure.Runtime;
using UCommerce.Pipelines;
using UCommerce.Pipelines.Initialization;

namespace Kaching.Extensions.Pipelines.MigrateDatabase
{
    public class MigrateDatabase : IPipelineTask<InitializeArgs>
    {
        private readonly IPathService _pathService;
        private readonly ILoggingService _loggingService;
        private readonly UCommerce.Installer.IInstallationConnectionStringLocator _connectionStringLocator;
        public MigrateDatabase(IPathService pathService, ILoggingService loggingService, UCommerce.Installer.IInstallationConnectionStringLocator connectionStringLocator)
        {
            _pathService = pathService;
            _loggingService = loggingService;
            _connectionStringLocator = connectionStringLocator;
        }


        public PipelineExecutionResult Execute(InitializeArgs subject)
        {
            //Find the virtual path to the Ucommerce root folder    
            string webpathToUCommerceRoot = _pathService.GetPath();

            //Map to the physical path
            string physicalPathToUCommerce = HostingEnvironment.MapPath(webpathToUCommerceRoot);

            //Join the sub folder where our migration scripts are located
            string pathToApp = Path.Combine(physicalPathToUCommerce, @"Apps\Ka-ching\contentFiles\any\net452\Database");

            //Use MigrationLoader to get the migrations
            IList<UCommerce.Installer.Migration> migrations = new UCommerce.Installer.MigrationLoader().GetDatabaseMigrations(new DirectoryInfo(pathToApp));

            //Create a new instance of the database installer
            var appsDatabaseInstaller = new UCommerce.Installer.AppsDatabaseInstaller(
                _connectionStringLocator,
                migrations,
                new InstallerLoggingServiceAdapter(_loggingService)
            );

            //Run the actual migration scripts
            appsDatabaseInstaller.InstallDatabase();

            return PipelineExecutionResult.Success;
        }
    }
}

