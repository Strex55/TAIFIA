'''
FizzBuzz — в цикле читает целые числа и печатает ответ, пока не встретит конец ввода
- если число делится на 3, то печатает Fizz
- если делится на 5, то печатает Buzz
- может напечатать “FizzBuzz”, если число делится как на 3, так и на 5
- если не делится ни на 3, ни на 5, то печатает само число
'''
def FizzBuzz():
    print("Введите целые числа (конец ввода: Ctrl+Z):")

    while True:
        try:
            line = input().strip()
        except EOFError:
            print("Конец ввода.")
            break

        try:
            n = int(line)
        except ValueError:
            print("Ошибка: нужно вводить целые числа.")
            continue

        if n % 3 == 0 and n % 5 == 0:
            print("FizzBuzz")
        elif n % 3 == 0:
            print("Fizz")
        elif n % 5 == 0:
            print("Buzz")
        else:
            print(n)


FizzBuzz()

