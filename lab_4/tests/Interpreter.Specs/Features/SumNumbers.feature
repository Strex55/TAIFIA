Feature: SumNumbers
  Программа складывает два дробных числа

  Scenario: Сложение двух чисел
    Given я написал программу:
      """
      bello!

      // Объявление переменных
      poop a Papaya naidu!      // первое число
      poop b Papaya naidu!      // второе число

      // Ввод данных
      guoleila (a) naidu!
      guoleila (b) naidu!

      // Вычисление суммы
      poop sum Papaya naidu!
      sum lumai a melomo b naidu!  // sum = a + b

      // Вывод результата
      tulalilloo ti amo (sum) naidu!
      """
    When я ввожу в консоли:
      """
      10
      20
      """
    Then я увижу в консоли:
      """
      30
      """

  Scenario: Сложение отрицательных чисел
    Given я написал программу:
      """
      bello!

      poop a Papaya naidu!
      poop b Papaya naidu!
      guoleila (a) naidu!
      guoleila (b) naidu!
      poop sum Papaya naidu!
      sum lumai a melomo b naidu!
      tulalilloo ti amo (sum) naidu!
      """
    When я ввожу в консоли:
      """
      -5
      15
      """
    Then я увижу в консоли:
      """
      10
      """

