Feature: Factorial
  Программа вычисляет факториал числа n (n!)

  Scenario: Факториал нуля
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Объявление функции "factorial", которая принимает число n и возвращает число n!
      boss factorial Papaya (n) oca!
          // Если n равно 0, то факториал 0! = 1
          bi-do (n con 0.0) oca!
              tank yu 1.0 naidu!  // Вернуть 1.0
          stopa
          // Иначе: n! = n * (n-1)!
          tank yu n dibotada factorial (n flavuk 1.0) naidu!
      stopa

      // Объявляем переменную для хранения входного числа
      poop n Papaya naidu!
      // Считываем число
      guoleila (n) naidu!
      // Вызываем функцию factorial и выводим результат
      tulalilloo ti amo (factorial (n)) naidu!
      """
    When я ввожу в консоли:
      """
      0
      """
    Then я увижу в консоли:
      """
      1
      """

  Scenario: Факториал единицы
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Объявление функции "factorial", которая принимает число n и возвращает число n!
      boss factorial Papaya (n) oca!
          // Если n равно 0, то факториал 0! = 1
          bi-do (n con 0.0) oca!
              tank yu 1.0 naidu!  // Вернуть 1.0
          stopa
          // Иначе: n! = n * (n-1)!
          tank yu n dibotada factorial (n flavuk 1.0) naidu!
      stopa

      // Объявляем переменную для хранения входного числа
      poop n Papaya naidu!
      // Считываем число
      guoleila (n) naidu!
      // Вызываем функцию factorial и выводим результат
      tulalilloo ti amo (factorial (n)) naidu!
      """
    When я ввожу в консоли:
      """
      1
      """
    Then я увижу в консоли:
      """
      1
      """

  Scenario: Факториал пяти
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Объявление функции "factorial", которая принимает число n и возвращает число n!
      boss factorial Papaya (n) oca!
          // Если n равно 0, то факториал 0! = 1
          bi-do (n con 0.0) oca!
              tank yu 1.0 naidu!  // Вернуть 1.0
          stopa
          // Иначе: n! = n * (n-1)!
          tank yu n dibotada factorial (n flavuk 1.0) naidu!
      stopa

      // Объявляем переменную для хранения входного числа
      poop n Papaya naidu!
      // Считываем число
      guoleila (n) naidu!
      // Вызываем функцию factorial и выводим результат
      tulalilloo ti amo (factorial (n)) naidu!
      """
    When я ввожу в консоли:
      """
      5
      """
    Then я увижу в консоли:
      """
      120
      """

  Scenario: Факториал семи
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Объявление функции "factorial", которая принимает число n и возвращает число n!
      boss factorial Papaya (n) oca!
          // Если n равно 0, то факториал 0! = 1
          bi-do (n con 0.0) oca!
              tank yu 1.0 naidu!  // Вернуть 1.0
          stopa
          // Иначе: n! = n * (n-1)!
          tank yu n dibotada factorial (n flavuk 1.0) naidu!
      stopa

      // Объявляем переменную для хранения входного числа
      poop n Papaya naidu!
      // Считываем число
      guoleila (n) naidu!
      // Вызываем функцию factorial и выводим результат
      tulalilloo ti amo (factorial (n)) naidu!
      """
    When я ввожу в консоли:
      """
      7
      """
    Then я увижу в консоли:
      """
      5040
      """

