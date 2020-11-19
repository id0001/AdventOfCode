from utils.io import read_by_character, print_solution


x, y = 0, 0
deliveries = dict()


def move_up():
    global y
    y -= 1


def move_right():
    global x
    x += 1


def move_down():
    global y
    y += 1


def move_left():
    global x
    x -= 1


def deliver():
    total = deliveries.get((x, y), 0)
    deliveries[(x, y)] = total + 1


moves = {
    '^': move_up,
    '>': move_right,
    'v': move_down,
    '<': move_left
}


for c in read_by_character(3):
    deliver()
    moves.get(c)()

print_solution(len(deliveries))
