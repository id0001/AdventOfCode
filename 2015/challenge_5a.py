from utils.io import print_solution, read_by_line

Vowels = ['a', 'e', 'i', 'o', 'u']
BadValues = ['ab', 'cd', 'pq', 'xy']


def vowel_count(value):
    return len(list(filter(lambda x: x in Vowels, value)))


def has_duplicate_letter(value):
    for i in range(1, len(value)):
        if value[i] == value[i-1]:
            return True
    return False


def does_not_contain(value, badValues):
    for v in badValues:
        if v in value:
            return False
    return True


def is_nice(line):
    # contains at least 3 vowels
    if not vowel_count(line) >= 3:
        return False

    # contains at least 1 repeating letter. eg: aa, bb, cc, dd
    if not has_duplicate_letter(line):
        return False

    # does not contain any of the following values: ab, cd, pq, xy
    return does_not_contain(line, BadValues)


nice = 0
for line in read_by_line(5):
    if is_nice(line):
        nice += 1

print_solution(nice)
