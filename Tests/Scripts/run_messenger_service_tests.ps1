# PowerShell script to run MessengerService tests

# Define paths
$solutionPath = "d:\Dev\Redot\Projects\Survival\Survival.sln"
$outputFile = "d:\Dev\Redot\Projects\Survival\Temp\Logs\MessengerServiceTests_$(Get-Date -Format yyyyMMdd_HHmmss).log"

# Run tests and log results
Write-Host "Running MessengerService tests..." -ForegroundColor Yellow
& dotnet test $solutionPath --filter "FullyQualifiedName~MessengerServiceTests" | Out-File $outputFile

Write-Host "MessengerService test results saved to $outputFile" -ForegroundColor Cyan