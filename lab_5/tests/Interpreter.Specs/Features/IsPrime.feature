Feature: IsPrime
  Программа проверяет, является ли число простым

  Scenario: Число меньше двух не является простым
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция is_prime: возвращает 1.0, если число простое, иначе 0.0
      boss is_prime Papaya (n) oca!
          // Числа меньше 2 не являются простыми
          bi-do (n la 2.0) oca!
              tank yu 0.0 naidu!
          stopa
          // Число 2 — простое
          bi-do (n con 2.0) oca!
              tank yu 1.0 naidu!
          stopa

          // Начинаем проверку делителей с числа 2
          poop i Papaya naidu!
          i lumai 2.0 naidu!

          // Пока i * i <= n (то есть i <= sqrt(n))
          kemari (i dibotada i lacon n) oca!
              // Если n делится на i без остатка — число составное
              bi-do (n pado i con 0.0) oca!
                  tank yu 0.0 naidu!  // Не простое
              stopa
              // Увеличиваем i на 1
              i lumai i melomo 1.0 naidu!
          stopa

          // Если ни один делитель не найден — число простое
          tank yu 1.0 naidu!
      stopa

      // Считываем число с клавиатуры
      poop n Papaya naidu!
      guoleila (n) naidu!
      // Выводим 1.0 (простое) или 0.0 (составное)
      tulalilloo ti amo (is_prime (n)) naidu!
      """
    When я ввожу в консоли:
      """
      1
      """
    Then я увижу в консоли:
      """
      0
      """

  Scenario: Число два является простым
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция is_prime: возвращает 1.0, если число простое, иначе 0.0
      boss is_prime Papaya (n) oca!
          // Числа меньше 2 не являются простыми
          bi-do (n la 2.0) oca!
              tank yu 0.0 naidu!
          stopa
          // Число 2 — простое
          bi-do (n con 2.0) oca!
              tank yu 1.0 naidu!
          stopa

          // Начинаем проверку делителей с числа 2
          poop i Papaya naidu!
          i lumai 2.0 naidu!

          // Пока i * i <= n (то есть i <= sqrt(n))
          kemari (i dibotada i lacon n) oca!
              // Если n делится на i без остатка — число составное
              bi-do (n pado i con 0.0) oca!
                  tank yu 0.0 naidu!  // Не простое
              stopa
              // Увеличиваем i на 1
              i lumai i melomo 1.0 naidu!
          stopa

          // Если ни один делитель не найден — число простое
          tank yu 1.0 naidu!
      stopa

      // Считываем число с клавиатуры
      poop n Papaya naidu!
      guoleila (n) naidu!
      // Выводим 1.0 (простое) или 0.0 (составное)
      tulalilloo ti amo (is_prime (n)) naidu!
      """
    When я ввожу в консоли:
      """
      2
      """
    Then я увижу в консоли:
      """
      1
      """

  Scenario: Простое число три
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция is_prime: возвращает 1.0, если число простое, иначе 0.0
      boss is_prime Papaya (n) oca!
          // Числа меньше 2 не являются простыми
          bi-do (n la 2.0) oca!
              tank yu 0.0 naidu!
          stopa
          // Число 2 — простое
          bi-do (n con 2.0) oca!
              tank yu 1.0 naidu!
          stopa

          // Начинаем проверку делителей с числа 2
          poop i Papaya naidu!
          i lumai 2.0 naidu!

          // Пока i * i <= n (то есть i <= sqrt(n))
          kemari (i dibotada i lacon n) oca!
              // Если n делится на i без остатка — число составное
              bi-do (n pado i con 0.0) oca!
                  tank yu 0.0 naidu!  // Не простое
              stopa
              // Увеличиваем i на 1
              i lumai i melomo 1.0 naidu!
          stopa

          // Если ни один делитель не найден — число простое
          tank yu 1.0 naidu!
      stopa

      // Считываем число с клавиатуры
      poop n Papaya naidu!
      guoleila (n) naidu!
      // Выводим 1.0 (простое) или 0.0 (составное)
      tulalilloo ti amo (is_prime (n)) naidu!
      """
    When я ввожу в консоли:
      """
      3
      """
    Then я увижу в консоли:
      """
      1
      """

  Scenario: Составное число четыре
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция is_prime: возвращает 1.0, если число простое, иначе 0.0
      boss is_prime Papaya (n) oca!
          // Числа меньше 2 не являются простыми
          bi-do (n la 2.0) oca!
              tank yu 0.0 naidu!
          stopa
          // Число 2 — простое
          bi-do (n con 2.0) oca!
              tank yu 1.0 naidu!
          stopa

          // Начинаем проверку делителей с числа 2
          poop i Papaya naidu!
          i lumai 2.0 naidu!

          // Пока i * i <= n (то есть i <= sqrt(n))
          kemari (i dibotada i lacon n) oca!
              // Если n делится на i без остатка — число составное
              bi-do (n pado i con 0.0) oca!
                  tank yu 0.0 naidu!  // Не простое
              stopa
              // Увеличиваем i на 1
              i lumai i melomo 1.0 naidu!
          stopa

          // Если ни один делитель не найден — число простое
          tank yu 1.0 naidu!
      stopa

      // Считываем число с клавиатуры
      poop n Papaya naidu!
      guoleila (n) naidu!
      // Выводим 1.0 (простое) или 0.0 (составное)
      tulalilloo ti amo (is_prime (n)) naidu!
      """
    When я ввожу в консоли:
      """
      4
      """
    Then я увижу в консоли:
      """
      0
      """

  Scenario: Простое число семнадцать
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция is_prime: возвращает 1.0, если число простое, иначе 0.0
      boss is_prime Papaya (n) oca!
          // Числа меньше 2 не являются простыми
          bi-do (n la 2.0) oca!
              tank yu 0.0 naidu!
          stopa
          // Число 2 — простое
          bi-do (n con 2.0) oca!
              tank yu 1.0 naidu!
          stopa

          // Начинаем проверку делителей с числа 2
          poop i Papaya naidu!
          i lumai 2.0 naidu!

          // Пока i * i <= n (то есть i <= sqrt(n))
          kemari (i dibotada i lacon n) oca!
              // Если n делится на i без остатка — число составное
              bi-do (n pado i con 0.0) oca!
                  tank yu 0.0 naidu!  // Не простое
              stopa
              // Увеличиваем i на 1
              i lumai i melomo 1.0 naidu!
          stopa

          // Если ни один делитель не найден — число простое
          tank yu 1.0 naidu!
      stopa

      // Считываем число с клавиатуры
      poop n Papaya naidu!
      guoleila (n) naidu!
      // Выводим 1.0 (простое) или 0.0 (составное)
      tulalilloo ti amo (is_prime (n)) naidu!
      """
    When я ввожу в консоли:
      """
      17
      """
    Then я увижу в консоли:
      """
      1
      """

  Scenario: Составное число двадцать пять
    Given я написал программу:
      """
      // Начало программы
      bello!

      // Функция is_prime: возвращает 1.0, если число простое, иначе 0.0
      boss is_prime Papaya (n) oca!
          // Числа меньше 2 не являются простыми
          bi-do (n la 2.0) oca!
              tank yu 0.0 naidu!
          stopa
          // Число 2 — простое
          bi-do (n con 2.0) oca!
              tank yu 1.0 naidu!
          stopa

          // Начинаем проверку делителей с числа 2
          poop i Papaya naidu!
          i lumai 2.0 naidu!

          // Пока i * i <= n (то есть i <= sqrt(n))
          kemari (i dibotada i lacon n) oca!
              // Если n делится на i без остатка — число составное
              bi-do (n pado i con 0.0) oca!
                  tank yu 0.0 naidu!  // Не простое
              stopa
              // Увеличиваем i на 1
              i lumai i melomo 1.0 naidu!
          stopa

          // Если ни один делитель не найден — число простое
          tank yu 1.0 naidu!
      stopa

      // Считываем число с клавиатуры
      poop n Papaya naidu!
      guoleila (n) naidu!
      // Выводим 1.0 (простое) или 0.0 (составное)
      tulalilloo ti amo (is_prime (n)) naidu!
      """
    When я ввожу в консоли:
      """
      25
      """
    Then я увижу в консоли:
      """
      0
      """

