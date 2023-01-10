#!/usr/bin/env bash

year=$1
day=$2
zeroDay=$(printf %02d $day)

templateFile="$PWD/AdventOfCode.Core/Scripts/template.txt"
challengeDir="$PWD/AdventOfCode.$year/Challenges"

template=$(<$templateFile)
template=${template//\{\{year\}\}/$year}
template=${template//\{\{zeroday\}\}/$zeroDay}
template=${template//\{\{day\}\}/$day}
echo $template >> "$challengeDir/Challenge$zeroDay.cs"