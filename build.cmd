mkdir "%~0\..\out"
call "%~0\..\nuget.cmd" pack package\DacpacDataMigrations.nuspec -output "%~0\..\out" -version 1.2.0.0