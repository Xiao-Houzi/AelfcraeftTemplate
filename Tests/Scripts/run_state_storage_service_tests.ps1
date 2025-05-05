# PowerShell script to run StateStorageService tests

# Define paths
$solutionPath = "d:\Dev\Redot\Projects\Aelfcraeft\Aelfcraeft.sln"
$outputFile = "d:\Dev\Redot\Projects\Aelfcraeft\Temp\Logs\StateStorageServiceTests_$(Get-Date -Format yyyyMMdd_HHmmss).log"

# Run tests and log results
Write-Host "Running StateStorageService tests..." -ForegroundColor Yellow
& dotnet test $solutionPath --filter "FullyQualifiedName~StateStorageServiceTests" | Out-File $outputFile

Write-Host "StateStorageService test results saved to $outputFile" -ForegroundColor Cyan