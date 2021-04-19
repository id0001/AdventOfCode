
from utils.io import print_solution, read_by_line


def calculate_required_paper(l, w, h):
    return (2*l*w) + (2*w*h) + (2*h*l) + min(l*w, w*h, h*l)


total_sqft = 0
for line in read_by_line(2):
    (l, w, h) = line.split('x')
    total_sqft += calculate_required_paper(int(l), int(w), int(h))

print_solution(total_sqft)
