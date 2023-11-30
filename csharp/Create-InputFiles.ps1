param([int]$Year)

$projectDir = "$PWD\\AdventOfCode.$Year"
$inputDir = "$projectDir\\Inputs"

if( -not (Test-Path -Path $projectDir)) {
    Write-Error "Directory for year $Year not found."
    exit
}

if( -not (Test-Path -Path $inputDir)) {
    New-Item -Path $inputDir -ItemType Directory
}

for($day = 0; $day -lt 26; $day++){
    $zeroDay = "{0:d2}" -f $day
    $dayFile = "$inputDir\\$zeroDay.txt"

    if( -not (Test-Path -Path $dayFile)) {
        New-Item -Path $dayFile -ItemType File
    }
}
