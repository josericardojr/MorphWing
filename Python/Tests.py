from kanren import run, eq, var
x = var()
print(run(1, x, eq(x, 5)))