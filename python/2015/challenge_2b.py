
from utils.io import print_solution, read_by_line


def calculate_required_ribbon(l, w, h):
    m = max(l, w, h)
    switcher = {
        l: w+w+h+h,
        w: l+l+h+h,
        h: l+l+w+w
    }

    return switcher.get(m) + (l*w*h)


total_ribbon = 0
for line in read_by_line(2):
    (l, w, h) = line.split('x')
    total_ribbon += calculate_required_ribbon(int(l), int(w), int(h))

print_solution(total_ribbon)
