# README #
Data migrations is not an easy task, few condition should be met if you want be able to do it without any problems. you can read it in migration scenarios.

Most of the time database deployment is done using database comparing tools and few script developers wrote. This could be error prone, stressful and hard to automate. With this package you can perform database updates in predictable manner. You can test it. Store and version your database package as an artifact on your CI.

##Installation:

Visual Studio does not supports nuget packages in SSDT projects. Since you cant add nuget to this kind of project to visual studio, you should do what visual studio is doing by hand. 

First you should create [packages.config](project-template-files/packages.config) in our database project folder to inform nuget to restore package.
After you restored package you can copy to database project folder file [DacpacDataMigrationsInclude.targets](project-template-files/DacpacDataMigrationsInclude.targets) and include it in sqlproj file, to do that add `<Import Condition="Exists('DacpacDataMigrationsInclude.targets')" Project="DacpacDataMigrationsInclude.targets" />`

When everything is set up, you can build your project. Migration will be added to your dacpac automatically.

##Usage:

In your database project folder after build it, you should notice folder named `DataMigrations`, you can add sql files to it, they should be marked with build action `None` if you want to add them to visual studio project. They are added to deployment script in alphabetical order, files can be nested in sub directories. you can force order by prefixing them with numbers 001, 002 ... etc.

##Running:

When you publish your database for the first time, table `__Migration.Log` will be created. Length of migration name is 256 characters. In this table migrations will be added when deployment is performed successfully. Schema and table name can be changed setting msbuild props `DacpacDataMigrationsSchema` and `DacpacDataMigrationsTableName` before including targets file.

Right now DDL and DML scripts are run in one transaction.
