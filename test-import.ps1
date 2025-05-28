# Test script for JSON import functionality
Write-Host "Testing DnD Adventure JSON Import Functionality" -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Green

# Read the test JSON file
$jsonContent = Get-Content -Path "Test-World.json" -Raw

# Test 1: Validate JSON
Write-Host "`n1. Testing JSON Validation..." -ForegroundColor Yellow
try {
    $validateBody = @{
        jsonContent = $jsonContent
    } | ConvertTo-Json -Depth 10

    $validateResponse = Invoke-WebRequest -Uri "http://localhost:5000/api/save/validate-json" -Method POST -Body $validateBody -ContentType "application/json"
    Write-Host "✅ JSON Validation: SUCCESS (Status: $($validateResponse.StatusCode))" -ForegroundColor Green
    Write-Host "Response: $($validateResponse.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ JSON Validation: FAILED" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Import JSON
Write-Host "`n2. Testing JSON Import..." -ForegroundColor Yellow
try {
    $importBody = @{
        jsonContent = $jsonContent
    } | ConvertTo-Json -Depth 10

    $importResponse = Invoke-WebRequest -Uri "http://localhost:5000/api/save/import" -Method POST -Body $importBody -ContentType "application/json"
    Write-Host "✅ JSON Import: SUCCESS (Status: $($importResponse.StatusCode))" -ForegroundColor Green
    Write-Host "Response: $($importResponse.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ JSON Import: FAILED" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: List available saves
Write-Host "`n3. Testing Save List..." -ForegroundColor Yellow
try {
    $listResponse = Invoke-WebRequest -Uri "http://localhost:5000/api/save" -Method GET
    Write-Host "✅ Save List: SUCCESS (Status: $($listResponse.StatusCode))" -ForegroundColor Green
    Write-Host "Available saves: $($listResponse.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Save List: FAILED" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Create quick save
Write-Host "`n4. Testing Quick Save..." -ForegroundColor Yellow
try {
    $quicksaveResponse = Invoke-WebRequest -Uri "http://localhost:5000/api/save/quicksave" -Method POST
    Write-Host "✅ Quick Save: SUCCESS (Status: $($quicksaveResponse.StatusCode))" -ForegroundColor Green
    Write-Host "Response: $($quicksaveResponse.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Quick Save: FAILED" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 5: List saves again to confirm
Write-Host "`n5. Testing Save List After Quick Save..." -ForegroundColor Yellow
try {
    $listResponse2 = Invoke-WebRequest -Uri "http://localhost:5000/api/save" -Method GET
    Write-Host "✅ Save List After Quick Save: SUCCESS (Status: $($listResponse2.StatusCode))" -ForegroundColor Green
    Write-Host "Available saves: $($listResponse2.Content)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Save List After Quick Save: FAILED" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=============================================" -ForegroundColor Green
Write-Host "Import test completed!" -ForegroundColor Green
