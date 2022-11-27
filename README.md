# Dacpac Data Migrations

## Whats new in 2.0.0

Now `DacpacDataMigrations` is an MSBuild SDK package, just like [Microsoft.Build.Sql](https://www.nuget.org/packages/Microsoft.Build.Sql)

## Installation

Add to your `sqlproj` file `Sdk` tag that points to the package `DacpacDataMigrations`. It will be downloaded automatically. You can specify version of this package in `Sdk` tag or in [global.json](https://learn.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk?view=vs-2022#how-project-sdks-are-resolved).

```xml
<Project DefaultTargets="Build">
  <Sdk Name="Microsoft.Build.Sql" Version="0.1.10-preview" />
  <Sdk Name="DacpacDataMigrations" Version="2.0.0" />

```

You could also add `DacpacDataMigrations` with `PackageReference` tag.

After setup migrations will be added to your `dacpac` automatically.

## Usage

After you build your database project, you should notice folder named `DataMigrations`, you can add `.sql` files to it, they are marked with build action `None` and are excluded from `SqlBuild` build action.

Files are added to deployment script in a `OrdinalCaseInsensitive` sort order, files can be nested in sub directories. You can force correct order for numbers by prefixing numbers with 0001, 0002 ... etc. Since 2 is after 10 in this sort order.

Folders can be useful when you want to group your migrations by some criteria, for example you can have folders related to a feature you are developing, or you can have folders for each sprint, or you can have folders for each release.

Remember that each migration file will be executed **only once**, so if you want to change something in a migration file after deployment, you should create a new one with a new name and start from the point of your last migration.

You can also add `PreDeploy.sql` and `PostDeploy.sql` files to a database project root directory, those files will be executed every time you deploy, before schema changes and after data migrations are complete.

## Publishing a database

When you publish your database for the first time, table `__Migration.LogV2` will be created. Length of migration name is 256 characters and it is a relative path to `DataMigrations` folder. In this table migrations will be added only when deployment is performed successfully. You can find there also a date and time when migration was performed and user who performed it. Right now DDL and DML scripts are run in one transaction.

## Migration from 1.0.0 to 2.0.0

In version 1.0.0 migrations were stored in `__Migration.Log` table by default, although you could customize name of a schema and table using `DacpacDataMigrationsSchema` and `DacpacDataMigrationsTableName`, in version 2.0.0 migrations are stored in `__Migration.LogV2` table and only schema name can be customized. Data from `__Migration.Log` table will be migrated to `__Migration.LogV2` table during first deployment using version 2.0.0.

## Configuration in MSBuild

Configurable properties are:

- `DacpacDataMigrationsSchema` - schema name where migrations are stored, default value is `__Migration`
- `SortMigrationFilesUsingNumericOrder` - if set to `true` migrations will be sorted using numeric order, default value is `false`. This mode will parse out numbers from paths and sort them numerically, so `2\2.sql` will be before `10\10.sql`.
