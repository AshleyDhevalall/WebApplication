$workingDir = $PSScriptRoot
$resultsDir = Join-Path $PSScriptRoot "testresults"

Write-Host $workingDir
Write-Host $resultsDir

Remove-Item $resultsDir -Recurse -Force -ErrorAction SilentlyContinue
#& dotnet tool install --global dotnet-sonarscanner
#& dotnet tool install --global dotnet-reportgenerator-globaltool

& dotnet test "$workingDir/WebApplication.Tests/WebApplication.Tests.csproj" /p:Exclude="[xunit.*]*" /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput="$resultsDir/coverage" --logger "trx;LogFileName=$resultsDir\results.trx"
& reportgenerator "-reports:$resultsDir/coverage.opencover.xml" "-targetdir:$resultsDir/reports" "-reporttypes:HTMLInline;HTMLChart"