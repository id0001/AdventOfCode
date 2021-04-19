from utils.io import read_by_character, print_solution
import collections as col


Position = col.namedtuple("Position", ['x', 'y'])

santa1 = Position(0, 0)
santa2 = Position(0, 0)

deliveries = dict()


def move_up(currentPosition):
    return Position(currentPosition.x, currentPosition.y-1)


def move_right(currentPosition):
    return Position(currentPosition.x + 1, currentPosition.y)


def move_down(currentPosition):
    return Position(currentPosition.x, currentPosition.y+1)


def move_left(currentPosition):
    return Position(currentPosition.x-1, currentPosition.y)


def deliver(currentPosition):
    total = deliveries.get(currentPosition, 0)
    deliveries[currentPosition] = total + 1


moves = {
    '^': move_up,
    '>': move_right,
    'v': move_down,
    '<': move_left
}

i = 0
for c in read_by_character(3):
    if i % 2 == 0:
        deliver(santa1)
        santa1 = moves.get(c)(santa1)
    else:
        deliver(santa2)
        santa2 = moves.get(c)(santa2)
    i += 1

print_solution(len(deliveries))
