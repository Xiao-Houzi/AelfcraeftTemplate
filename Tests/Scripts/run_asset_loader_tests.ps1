# PowerShell script to run AssetLoaderService tests

# Define paths
$solutionPath = "d:\Dev\Redot\Projects\Survival\Survival.sln"
$outputFile = "d:\Dev\Redot\Projects\Survival\Temp\Logs\AssetLoaderServiceTests_$(Get-Date -Format yyyyMMdd_HHmmss).log"

# Run tests and log results
Write-Host "Running AssetLoaderService tests..." -ForegroundColor Yellow
& dotnet test $solutionPath --filter "FullyQualifiedName~AssetLoaderServiceTests" | Out-File $outputFile

Write-Host "AssetLoaderService test results saved to $outputFile" -ForegroundColor Cyan