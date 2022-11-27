#Requires –Modules SqlServer, Pester

Describe "DacpacDataMigrations" {
  BeforeAll {
    $dacpac = "$PSScriptRoot\tests\TestProject\TestProject\bin\Debug\TestProject.dacpac"
    $server = "(localdb)\MSSQLLocalDB"
    $database = "TestProject"

    $report = New-TemporaryFile

    & {
      dotnet tool restore
      & "$PSScriptRoot\build.ps1" -VersionSuffix "Test.$((Get-Date -Format yyyyMMdd.HHmmss) -replace "\.0+", ".")"
      $LASTEXITCODE | Should -Be 0 -Because "Build should succeed"

      $version = Get-ChildItem "$PSScriptRoot\out" -Filter "DacpacDataMigrations.*.nupkg"
      | Select-Object -Last 1 -ExpandProperty Name
      | Select-String "DacpacDataMigrations\.(?<version>.+)\.nupkg"
      | ForEach-Object { $_.Matches[0].Groups['version'].Value }

      Write-Host "Using version '$version'"
      Remove-Item "$PSScriptRoot\tests\TestProject\packages" -Recurse -Force -ErrorAction SilentlyContinue
      dotnet build "$PSScriptRoot\tests\TestProject\TestProject.sln" -bl -p:DacpackDataMigrationsVersion=$version
      dotnet tool run sqlpackage /Action:Publish /DeployReportPath:$report /SourceFile:$dacpac /TargetDatabaseName:$database /TargetServerName:$server /p:CreateNewDatabase=True

    } | Write-Verbose
  }
  It "should not deploy anything when database is up to date" {

    & {
      $report = New-TemporaryFile
      dotnet tool run sqlpackage /Action:Publish /DeployReportPath:$report /SourceFile:$dacpac /TargetDatabaseName:$database /TargetServerName:$server

      [xml]$reportData = Get-Content $report

      $reportData.DeploymentReport.Alerts | Should -BeNullOrEmpty
      $result = $reportData.DeploymentReport.Operations.Operation | Select-Object -Property @{Name = 'OperationName'; Expression = { $_.Name } } -ExpandProperty Item

      $result | Should -HaveCount 0

    } | Write-Verbose
  }
  It "msbuild task should pass tests" {
    & {
      dotnet test $PSScriptRoot\DacpackDataMigrations.sln
      $LASTEXITCODE | Should -Be 0 -Because "Tests should succeed"
    } | Write-Verbose
  }
  It "should deploy database and apply all migrations" {
    & {
      try {

        [xml]$reportData = Get-Content $report

        $reportData.DeploymentReport.Alerts | Should -BeNullOrEmpty
        $result = $reportData.DeploymentReport.Operations.Operation | Select-Object -Property @{Name = 'OperationName'; Expression = { $_.Name } } -ExpandProperty Item

        $result | Should -HaveCount 1
        $result.OperationName | Should -Be 'Create'
        $result.Value | Should -Be '[dbo].[Table1]'
        $result.Type | Should -Be 'SqlTable'

        $result = Invoke-Sqlcmd -ServerInstance $server -Database $database -Query 'select * from Table1'

        $result.Id | Should -Be @(1, 2, 3)

        $result = Invoke-Sqlcmd -ServerInstance $server -Database $database -Query 'select * from __Migrations.LogV2'
        $result.Id | Should -Be @(1, 2, 3)
        $result.Name | Should -Be @('001 Seed Table1', '2 test order', '10 test order')

      } finally {
        Pop-Location
      }
    } | Write-Verbose
  }
}
