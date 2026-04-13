Feature: CircleSquare
  Программа вычисляет площадь круга по радиусу

  Scenario: Вычисление площади круга
    Given я написал программу:
      """
      bello!

      // Константа: число пи
      trusela pi Papaya belloPi naidu!

      // Переменная для радиуса
      poop radius Papaya naidu!

      // Ввод радиуса
      guoleila (radius) naidu!

      // Вычисление площади
      poop area Papaya naidu!
      area lumai pi dibotada (radius beedo 2) naidu!  // S = π * (r^2)

      // Вывод результата
      tulalilloo ti amo (area) naidu!
      """
    When я ввожу в консоли:
      """
      1
      """
    Then я увижу в консоли:
      """
      3.14159
      """

  Scenario: Вычисление площади круга с радиусом 5
    Given я написал программу:
      """
      bello!

      trusela pi Papaya belloPi naidu!
      poop radius Papaya naidu!
      guoleila (radius) naidu!
      poop area Papaya naidu!
      area lumai pi dibotada (radius beedo 2) naidu!
      tulalilloo ti amo (area) naidu!
      """
    When я ввожу в консоли:
      """
      5
      """
    Then я увижу в консоли:
      """
      78.53982
      """

