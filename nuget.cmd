if not exist "%~0\..\.nuget\nuget.exe" (
  md "%~0\..\.nuget"
  curl -o "%~0\..\.nuget\nuget.exe" -L http://www.nuget.org/nuget.exe
  %~0\..\.nuget\nuget.exe update -self
)
"%~0\..\.nuget\nuget.exe" %*