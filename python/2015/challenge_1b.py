from utils.io import print_solution, read_by_character

floor = 0
position = 1
for c in read_by_character(1):
    floor += 1 if c == '(' else -1
    if floor < 0:
        break
    position += 1

print_solution(position)
