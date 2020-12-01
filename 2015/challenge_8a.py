from os import read
from utils.io import print_solution, read_by_line
import re

count_memory = 0
count_literal = 0

for line in read_by_line(8):
    count_memory += len(line) + 2
    count_literal += len(line.encode("latin1").decode("unicode_escape"))

print_solution(count_memory - count_literal)
