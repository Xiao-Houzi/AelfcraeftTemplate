# PowerShell script to run ConfigLoader tests

# Define paths
$solutionPath = "d:\Dev\Redot\Projects\Aelfcraeft\Aelfcraeft.sln"
$outputFile = "d:\Dev\Redot\Projects\Aelfcraeft\Temp\Logs\ConfigLoaderTests_$(Get-Date -Format yyyyMMdd_HHmmss).log"

# Run tests and log results
Write-Host "Running ConfigLoader tests..." -ForegroundColor Yellow
& dotnet test $solutionPath --filter "FullyQualifiedName~ConfigLoaderTests" | Out-File $outputFile

Write-Host "ConfigLoader test results saved to $outputFile" -ForegroundColor Cyan