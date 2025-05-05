# PowerShell script to run all test scripts and gather logs

# Updated script to dynamically search for test scripts matching the `run_*_tests.ps1` format
$logDir = "d:\Dev\Redot\Projects\Survival\Temp\Logs"
$outputFile = "$logDir\MasterTestResults_$(Get-Date -Format 'yyyyMMdd_HHmmss').log"

# Ensure log directory exists and remove only old test log files
if (-Not (Test-Path $logDir)) {
    New-Item -ItemType Directory -Path $logDir | Out-Null
} else {
    # Remove only old test log files matching the pattern *Tests*.log
    Get-ChildItem -Path $logDir -File | Where-Object { $_.Name -like "*Tests*.log" } | Remove-Item -Force
}

# Initialize master log
"Master Test Results - $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" | Out-File $outputFile
"=============================================" | Out-File $outputFile -Append

# Dynamically search for test scripts
$testScripts = Get-ChildItem -Path "d:\Dev\Redot\Projects\TemplateProject\Tests\Scripts" -Filter "*.cs" -Recurse

foreach ($scriptPath in $testScripts) {
    Write-Host "Running $scriptPath..." -ForegroundColor Yellow
    try {
        & $scriptPath | Out-File -Append -FilePath $outputFile
    } catch {
        Write-Host "Error running $scriptPath" -ForegroundColor Red
        "Error running $($scriptPath): An error occurred." | Out-File -Append -FilePath $outputFile
    }
}

# Gather all individual test results into the master results file
$individualLogs = Get-ChildItem -Path $logDir -File | Where-Object { $_.Name -like "*Tests*.log" }

"\nAggregated Test Results:\n" | Out-File -Append -FilePath $outputFile
foreach ($log in $individualLogs) {
    "\n--- Results from $($log.Name) ---\n" | Out-File -Append -FilePath $outputFile
    Get-Content $log.FullName | Out-File -Append -FilePath $outputFile
}

Write-Host "Master test results saved to $outputFile" -ForegroundColor Cyan