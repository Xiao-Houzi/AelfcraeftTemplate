# PowerShell script to run BaseService tests

# Define paths
$solutionPath = "d:\Dev\Redot\Projects\Survival\Survival.sln"
$outputFile = "d:\Dev\Redot\Projects\Survival\Temp\Logs\BaseServiceTests_$(Get-Date -Format yyyyMMdd_HHmmss).log"

# Run tests and log results
Write-Host "Running BaseService tests..." -ForegroundColor Yellow
& dotnet test $solutionPath --filter "FullyQualifiedName~BaseServiceTests" | Out-File $outputFile

Write-Host "BaseService test results saved to $outputFile" -ForegroundColor Cyan