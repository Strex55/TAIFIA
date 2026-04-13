Feature: Pow
  Программа возводит число в целую неотрицательную степень

  Scenario: Возведение в нулевую степень
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция pow: возводит base в целую неотрицательную степень exp
      boss pow Papaya (base, exp) oca!
          // Начальное значение результата — 1
          poop result Papaya naidu!
          result lumai 1.0 naidu!

          // Счётчик итераций: начинаем с 0
          poop i Papaya naidu!
          i lumai 0.0 naidu!

          // Повторяем exp раз: умножаем result на base
          kemari (i la exp) oca!
              result lumai result dibotada base naidu!  // result = result * base
              i lumai i melomo 1.0 naidu!               // i = i + 1
          stopa

          // Возвращаем итоговый результат
          tank yu result naidu!
      stopa

      // Считываем основание степени
      poop a Papaya naidu!
      guoleila (a) naidu!
      // Считываем показатель степени
      poop b Papaya naidu!
      guoleila (b) naidu!
      // Выводим результат: a^b
      tulalilloo ti amo (pow (a, b)) naidu!
      """
    When я ввожу в консоли:
      """
      5
      0
      """
    Then я увижу в консоли:
      """
      1
      """

  Scenario: Возведение в первую степень
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция pow: возводит base в целую неотрицательную степень exp
      boss pow Papaya (base, exp) oca!
          // Начальное значение результата — 1
          poop result Papaya naidu!
          result lumai 1.0 naidu!

          // Счётчик итераций: начинаем с 0
          poop i Papaya naidu!
          i lumai 0.0 naidu!

          // Повторяем exp раз: умножаем result на base
          kemari (i la exp) oca!
              result lumai result dibotada base naidu!  // result = result * base
              i lumai i melomo 1.0 naidu!               // i = i + 1
          stopa

          // Возвращаем итоговый результат
          tank yu result naidu!
      stopa

      // Считываем основание степени
      poop a Papaya naidu!
      guoleila (a) naidu!
      // Считываем показатель степени
      poop b Papaya naidu!
      guoleila (b) naidu!
      // Выводим результат: a^b
      tulalilloo ti amo (pow (a, b)) naidu!
      """
    When я ввожу в консоли:
      """
      7
      1
      """
    Then я увижу в консоли:
      """
      7
      """

  Scenario: Возведение два в степень три
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция pow: возводит base в целую неотрицательную степень exp
      boss pow Papaya (base, exp) oca!
          // Начальное значение результата — 1
          poop result Papaya naidu!
          result lumai 1.0 naidu!

          // Счётчик итераций: начинаем с 0
          poop i Papaya naidu!
          i lumai 0.0 naidu!

          // Повторяем exp раз: умножаем result на base
          kemari (i la exp) oca!
              result lumai result dibotada base naidu!  // result = result * base
              i lumai i melomo 1.0 naidu!               // i = i + 1
          stopa

          // Возвращаем итоговый результат
          tank yu result naidu!
      stopa

      // Считываем основание степени
      poop a Papaya naidu!
      guoleila (a) naidu!
      // Считываем показатель степени
      poop b Papaya naidu!
      guoleila (b) naidu!
      // Выводим результат: a^b
      tulalilloo ti amo (pow (a, b)) naidu!
      """
    When я ввожу в консоли:
      """
      2
      3
      """
    Then я увижу в консоли:
      """
      8
      """

  Scenario: Возведение три в степень четыре
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция pow: возводит base в целую неотрицательную степень exp
      boss pow Papaya (base, exp) oca!
          // Начальное значение результата — 1
          poop result Papaya naidu!
          result lumai 1.0 naidu!

          // Счётчик итераций: начинаем с 0
          poop i Papaya naidu!
          i lumai 0.0 naidu!

          // Повторяем exp раз: умножаем result на base
          kemari (i la exp) oca!
              result lumai result dibotada base naidu!  // result = result * base
              i lumai i melomo 1.0 naidu!               // i = i + 1
          stopa

          // Возвращаем итоговый результат
          tank yu result naidu!
      stopa

      // Считываем основание степени
      poop a Papaya naidu!
      guoleila (a) naidu!
      // Считываем показатель степени
      poop b Papaya naidu!
      guoleila (b) naidu!
      // Выводим результат: a^b
      tulalilloo ti amo (pow (a, b)) naidu!
      """
    When я ввожу в консоли:
      """
      3
      4
      """
    Then я увижу в консоли:
      """
      81
      """

  Scenario: Возведение пять в степень три
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция pow: возводит base в целую неотрицательную степень exp
      boss pow Papaya (base, exp) oca!
          // Начальное значение результата — 1
          poop result Papaya naidu!
          result lumai 1.0 naidu!

          // Счётчик итераций: начинаем с 0
          poop i Papaya naidu!
          i lumai 0.0 naidu!

          // Повторяем exp раз: умножаем result на base
          kemari (i la exp) oca!
              result lumai result dibotada base naidu!  // result = result * base
              i lumai i melomo 1.0 naidu!               // i = i + 1
          stopa

          // Возвращаем итоговый результат
          tank yu result naidu!
      stopa

      // Считываем основание степени
      poop a Papaya naidu!
      guoleila (a) naidu!
      // Считываем показатель степени
      poop b Papaya naidu!
      guoleila (b) naidu!
      // Выводим результат: a^b
      tulalilloo ti amo (pow (a, b)) naidu!
      """
    When я ввожу в консоли:
      """
      5
      3
      """
    Then я увижу в консоли:
      """
      125
      """

