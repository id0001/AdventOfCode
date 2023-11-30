param([int]$Year)

$projectDir = "$PWD\\AdventOfCode.$Year"
$challengeDir = "$projectDir\\Challenges"
$templateFile = "$PWD\\AdventOfCode.Core\\Templates\\template.txt"

if( -not (Test-Path -Path $projectDir)) {
    Write-Error "Directory for year $Year not found."
    exit
}

if( -not (Test-Path -Path $challengeDir)) {
    New-Item -Path $challengeDir -ItemType Directory
}

for($day = 1; $day -lt 26; $day++){
    $zeroDay = "{0:d2}" -f $day
    $dayFile = "$challengeDir\\Challenge$zeroDay.cs"

    if( -not (Test-Path -Path $dayFile)) {
        $template = Get-Content -Path $templateFile
        $template = $template.Replace("{{year}}", $Year)
        $template = $template.Replace("{{zeroday}}", $zeroDay)
        $template = $template.Replace("{{day}}", $day)
        Set-Content -Path $dayFile -Value $template
    }
}