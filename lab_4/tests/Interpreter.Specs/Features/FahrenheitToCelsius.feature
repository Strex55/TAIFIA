Feature: FahrenheitToCelsius
  Программа переводит температуру из шкалы Фаренгейта в Цельсия

  Scenario: Перевод температуры из Фаренгейта в Цельсия
    Given я написал программу:
      """
      bello!

      // Константы формулы
      trusela offset Papaya 32.0 naidu!   // смещение 32°F
      trusela scale Papaya 1.8 naidu!     // масштабный коэффициент

      // Переменная для входной температуры
      poop f Papaya naidu!

      // Ввод температуры в °F
      guoleila (f) naidu!

      // Вычисление температуры в °C
      poop c Papaya naidu!
      c lumai (f flavuk offset) poopaye scale naidu!  // C = (F - 32) / 1.8

      // Вывод результата
      tulalilloo ti amo (c) naidu!
      """
    When я ввожу в консоли:
      """
      32
      """
    Then я увижу в консоли:
      """
      0
      """

  Scenario: Перевод 212 градусов Фаренгейта
    Given я написал программу:
      """
      bello!

      trusela offset Papaya 32.0 naidu!
      trusela scale Papaya 1.8 naidu!
      poop f Papaya naidu!
      guoleila (f) naidu!
      poop c Papaya naidu!
      c lumai (f flavuk offset) poopaye scale naidu!
      tulalilloo ti amo (c) naidu!
      """
    When я ввожу в консоли:
      """
      212
      """
    Then я увижу в консоли:
      """
      100
      """

