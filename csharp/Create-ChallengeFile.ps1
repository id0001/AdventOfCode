param([int]$Year, [int]$Day)

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


$zeroDay = "{0:d2}" -f $Day
$dayFile = "$challengeDir\\Challenge$zeroDay.cs"

if( -not (Test-Path -Path $dayFile)) {
	$template = Get-Content -Path $templateFile
	$template = $template.Replace("{{year}}", $Year)
	$template = $template.Replace("{{zeroday}}", $zeroDay)
	$template = $template.Replace("{{day}}", $Day)
	Set-Content -Path $dayFile -Value $template
}
