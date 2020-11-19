import hashlib
from utils.io import print_solution

key = "ckczppom"

i = 0
while True:
    i += 1
    hash = hashlib.md5((key + str(i)).encode("utf-8")).hexdigest()
    if hash.startswith('0' * 5):
        break

print_solution(i)
