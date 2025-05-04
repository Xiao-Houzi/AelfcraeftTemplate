# PowerShell script to run InputService tests

# Define paths
$solutionPath = "d:\Dev\Redot\Projects\Survival\Survival.sln"
$outputFile = "d:\Dev\Redot\Projects\Survival\Temp\Logs\InputServiceTests_$(Get-Date -Format yyyyMMdd_HHmmss).log"

# Run tests and log results
Write-Host "Running InputService tests..." -ForegroundColor Yellow
& dotnet test $solutionPath --filter "FullyQualifiedName~InputServiceTests" | Out-File $outputFile

Write-Host "InputService test results saved to $outputFile" -ForegroundColor Cyan