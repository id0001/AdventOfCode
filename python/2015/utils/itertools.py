

def groupby(fn, iter):
    res = dict()
    for item in iter:
        key = fn(item)
        if not key in res:
            res[key] = list()

        res[key].append(item)
    return res
