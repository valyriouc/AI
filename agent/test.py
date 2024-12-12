input = "print('\n'.join([(' ' * i) + ('*' * (2 * n - i - 1)) + '|' + ('*' * (2 * n - i - 1)) for i in range(n)])print('*' * (2*n-1))"

cleaned = input.replace("\n", "")
eval(cleaned)