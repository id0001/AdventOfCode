from utils.io import print_solution, read_by_line
from utils.itertools import groupby

Vowels = ['a', 'e', 'i', 'o', 'u']
BadValues = ['ab', 'cd', 'pq', 'xy']


def filter_pairs(value):
    pairs = list()
    last_added_index = -1
    for i in range(1, len(value)):
        pair = value[i-1] + value[i]

        if len(pairs) > 0 and pairs[-1] == pair and i - last_added_index < 2:
            continue

        pairs.append(pair)
        last_added_index = i
    return pairs


def contains_at_least_twice_repeating_pair(value):
    grouped = groupby(lambda x: x, filter_pairs(value))
    for key in grouped:
        if len(grouped[key]) >= 2:
            return True
    return False


def contains_at_least_one_gapped_pair(value):
    for i in range(2, len(value)):
        if value[i] == value[i-2]:
            return True
    return False


def is_nice(line):
    return contains_at_least_twice_repeating_pair(line) and contains_at_least_one_gapped_pair(line)


nice = 0
for line in read_by_line(5):
    if is_nice(line):
        nice += 1

print_solution(nice)
