#!/bin/bash

for i in {0..25}
do
	filename=`printf "%02s.txt" "$i"`
	touch $filename
done