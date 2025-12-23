# CircleSquare — читает радиус круга и печатает его площадь

import math

def CircleSquare():
    try:
        s = input("Введите радиус круга: ").strip()
        r = float(s)
        if r < 0:
            print("Радиус не может быть отрицательным.")
            return
    except ValueError:
        print("Неверный ввод — введите число.")
        return

    area = math.pi * r ** 2
    print("Площадь круга:", area)

CircleSquare()
