[CmdletBinding()]
param (
  [Parameter()]
  $Version = "2.0.0",
  [Parameter()]
  $VersionSuffix = ""
)

Remove-Item "$PSScriptRoot\out" -Force -Recurse -Confirm:$false

mkdir "$PSScriptRoot\out" | Out-Null

dotnet pack .\DacpacDataMigrations\DacpacDataMigrations.csproj -p:VersionPrefix=$Version -p:VersionSuffix=$VersionSuffix -o "$PSScriptRoot\out" -bl

