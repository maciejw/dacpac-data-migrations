cls

call build.cmd

pushd tests\TestProject

rd /s/q packages

xcopy /y ..\..\project-template-files\* TestProject

nuget restore -noCache

"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" TestProject.sln

popd