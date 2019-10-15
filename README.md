# Dacpac Data Migrations

## Installation

Visual Studio does not supports nuget packages in SSDT projects. Since you cannot add nuget to this kind of project to visual studio, you should do it by hand what Visual Studio is doing. 

First you should create [packages.config](project-template-files/packages.config) in our database project folder to inform nuget to restore package.
After you restored package you can copy to database project folder file [DacpacDataMigrationsInclude.targets](project-template-files/DacpacDataMigrationsInclude.targets) and include it in sqlproj file, to do that add `<Import Condition="Exists('DacpacDataMigrationsInclude.targets')" Project="DacpacDataMigrationsInclude.targets" />`.

To upgrade package you should update verion in packages.config and DacpacDataMigrationsInclude.targets files and restore packages again.

When everything is set up, you can build your project. Migrations will be added to your dacpac automatically.

## Usage

In your database project folder after build it, you should notice folder named `DataMigrations`, you can add sql files to it, they should be marked with build action `None` if you want to add them to visual studio project. They are added to deployment script in alphabetical order, files can be nested in sub directories. you can force order by prefixing them with numbers 001, 002 ... etc.

You can also add PreDeploy.sql and PostDeploy.sql files to database project root directory, this files will be run every time you deploy, before schema changes and after data migrations are finished.

## Running

When you publish your database for the first time, table `__Migration.Log` will be created. Length of migration name is 256 characters. In this table migrations will be added when deployment is performed successfully. Schema and table name can be changed setting msbuild props `DacpacDataMigrationsSchema` and `DacpacDataMigrationsTableName` before including targets file.

Right now DDL and DML scripts are run in one transaction.
