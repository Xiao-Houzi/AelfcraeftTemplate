# PowerShell script to run LogService tests

# Define paths
$solutionPath = "d:\Dev\Redot\Projects\Survival\Survival.sln"
$outputFile = "d:\Dev\Redot\Projects\Survival\Temp\Logs\LogServiceTests_$(Get-Date -Format yyyyMMdd_HHmmss).log"

# Run tests and log results
Write-Host "Running LogService tests..." -ForegroundColor Yellow
& dotnet test $solutionPath --filter "FullyQualifiedName~LogServiceTests" | Out-File $outputFile

Write-Host "LogService test results saved to $outputFile" -ForegroundColor Cyan