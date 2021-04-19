import io


def print_solution(solution):
    print("\n=============================================================")
    print("The solution is:")
    print(solution)


def read_as_string(challenge):
    with open_challenge_file(challenge) as f:
        return f.read()


def read_by_character(challenge):
    with open_challenge_file(challenge) as f:
        while True:
            c = f.read(1)
            if not c:
                break
            yield c


def read_by_line(challenge):
    with open_challenge_file(challenge) as f:
        while True:
            line = f.readline()
            if not line:
                break
            yield line


def create_path_for_challenge(challenge):
    return f".\\inputs\\{challenge}.txt"


def open_challenge_file(challenge):
    path = create_path_for_challenge(challenge)
    return open(path, 'r')
