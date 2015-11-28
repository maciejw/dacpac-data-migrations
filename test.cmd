cls

call build.cmd

set msbuild="C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
set sqlpackage="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\120\sqlpackage.exe"

pushd tests\TestProject

rd /s/q packages

xcopy /y ..\..\project-template-files\* TestProject

nuget restore -noCache

%msbuild% TestProject.sln

%sqlpackage% /Action:Publish /SourceFile:"TestProject\bin\Debug\TestProject.dacpac" /TargetDatabaseName:TestProject /TargetServerName:"(localdb)\MSSQLLocalDb" /p:CreateNewDatabase=True 
%sqlpackage% /Action:Publish /SourceFile:"TestProject\bin\Debug\TestProject.dacpac" /TargetDatabaseName:TestProject /TargetServerName:"(localdb)\MSSQLLocalDb"
popd
