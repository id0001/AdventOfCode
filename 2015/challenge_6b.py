from utils.io import print_solution, read_by_line
import collections as col

instructions = list()
matrix = [0 for i in range(1000*1000)]


def matrix_index(x, y):
    return (y * 1000) + x


def turn_on(matrix, start, end):
    for y in range(start.y, end.y):
        for x in range(start.x, end.x):
            matrix[matrix_index(x, y)] += 1


def turn_off(matrix, start, end):
    for y in range(start.y, end.y):
        for x in range(start.x, end.x):
            matrix[matrix_index(x, y)] = max(0, matrix[matrix_index(x, y)] - 1)


def toggle(matrix, start, end):
    for y in range(start.y, end.y):
        for x in range(start.x, end.x):
            matrix[matrix_index(x, y)] += 2


def create_instruction(value):
    split = value.split()

    if len(split) == 4:
        action = 2
        (x, y) = split[1].split(',')
        start = Point(int(x), int(y))
        (x, y) = split[3].split(',')
        end = Point(int(x)+1, int(y)+1)
    else:
        action = 0 if split[1] == 'on' else 1
        (x, y) = split[2].split(',')
        start = Point(int(x), int(y))
        (x, y) = split[4].split(',')
        end = Point(int(x)+1, int(y)+1)

    return Instruction(action, start, end)


Action = {
    0: turn_on,
    1: turn_off,
    2: toggle
}

Point = col.namedtuple('Point', ['x', 'y'])
Instruction = col.namedtuple("Instruction", ['action', 'start', 'end'])


for line in read_by_line(6):
    instructions.append(create_instruction(line))


for i in instructions:
    Action.get(i.action)(matrix, i.start, i.end)

print_solution(sum(list(filter(lambda x: x, matrix))))
