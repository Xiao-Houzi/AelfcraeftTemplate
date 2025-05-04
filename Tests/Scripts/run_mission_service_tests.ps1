# PowerShell script to run MissionService tests

# Define paths
$solutionPath = "d:\Dev\Redot\Projects\Survival\Survival.sln"
$outputFile = "d:\Dev\Redot\Projects\Survival\Temp\Logs\MissionServiceTests_$(Get-Date -Format yyyyMMdd_HHmmss).log"

# Run tests and log results
Write-Host "Running MissionService tests..." -ForegroundColor Yellow
& dotnet test $solutionPath --filter "FullyQualifiedName~MissionServiceTests" | Out-File $outputFile

Write-Host "MissionService test results saved to $outputFile" -ForegroundColor Cyan