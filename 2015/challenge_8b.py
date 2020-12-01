from utils.io import print_solution, read_by_line

count_memory = 0
count_literal = 0
count_encoded = 0

for line in read_by_line(0):
    count_memory += len(line) + 2
    count_literal += len(line.encode("latin1").decode("unicode_escape"))
    count_encoded += len(line) + 6 + line.count('\\\\') + line.count('\\\"')

print(count_memory)
print(count_literal)
print(count_encoded)
print_solution(count_memory - count_literal)
print_solution(count_encoded - count_literal)
