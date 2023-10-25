param([int]$Year, [int]$Day)

$zeroDay = "{0:d2}" -f $Day

$templateFile = "$PWD\\AdventOfCode.Core\\Scripts\\template.txt"
$challengeDir = "$PWD\\AdventOfCode.$Year\\Challenges"

$template = Get-Content -Path $templateFile
$template = $template.Replace("{{year}}", $Year)
$template = $template.Replace("{{zeroday}}", $zeroDay)
$template = $template.Replace("{{day}}", $Day)
Set-Content -Path "$challengeDir\\Challenge$zeroDay.cs" -Value $template