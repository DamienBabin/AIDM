# PowerShell script to test JSON import functionality

# Read the test JSON file
$jsonContent = Get-Content -Path "Test-World.json" -Raw

# Create the request body
$requestBody = @{
    jsonContent = $jsonContent
} | ConvertTo-Json -Depth 10

# Test validation endpoint
Write-Host "Testing JSON validation..."
try {
    $validationResult = Invoke-RestMethod -Uri "http://localhost:5000/api/save/validate-json" -Method POST -Body $requestBody -ContentType "application/json"
    Write-Host "Validation successful:"
    $validationResult | ConvertTo-Json -Depth 5
} catch {
    Write-Host "Validation failed:"
    Write-Host $_.Exception.Message
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response body: $responseBody"
    }
}

Write-Host "`n" + "="*50 + "`n"

# Test import endpoint
Write-Host "Testing JSON import..."
try {
    $importResult = Invoke-RestMethod -Uri "http://localhost:5000/api/save/import" -Method POST -Body $requestBody -ContentType "application/json"
    Write-Host "Import successful:"
    Write-Host "World Name: $($importResult.name)"
    Write-Host "Characters: $($importResult.characters.Count)"
    Write-Host "NPCs: $($importResult.npcs.Count)"
} catch {
    Write-Host "Import failed:"
    Write-Host $_.Exception.Message
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response body: $responseBody"
    }
}

Write-Host "`n" + "="*50 + "`n"

# Test getting saves
Write-Host "Testing get saves..."
try {
    $saves = Invoke-RestMethod -Uri "http://localhost:5000/api/save" -Method GET
    Write-Host "Available saves:"
    $saves | ForEach-Object { Write-Host "- $($_.worldName) ($($_.fileName))" }
} catch {
    Write-Host "Get saves failed:"
    Write-Host $_.Exception.Message
}

Write-Host "`n" + "="*50 + "`n"

# Test quicksave
Write-Host "Testing quicksave..."
try {
    $quicksaveResult = Invoke-RestMethod -Uri "http://localhost:5000/api/save/quicksave" -Method POST
    Write-Host "Quicksave successful:"
    $quicksaveResult | ConvertTo-Json
} catch {
    Write-Host "Quicksave failed:"
    Write-Host $_.Exception.Message
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseBody = $reader.ReadToEnd()
        Write-Host "Response body: $responseBody"
    }
}
