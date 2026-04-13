# Reqnroll Gherkin тесты для интерпретатора Minion#

Этот проект содержит BDD тесты на основе Reqnroll и Gherkin для проверки работы интерпретатора Minion#.

## Структура

- `Features/` - Gherkin feature файлы с описанием тестов
- `StepDefinitions/` - Реализация шагов тестов на C#

## Запуск тестов

```bash
dotnet test tests/Interpreter.Specs/Interpreter.Specs.csproj
```

## Формат тестов

Каждый тест содержит три шага:

1. **Given (Пусть)** - описание программы на языке Minion#
2. **When (Когда)** - входные данные для программы
3. **Then (Тогда)** - ожидаемый вывод программы

## Пример

```gherkin
Scenario: Сложение двух чисел
  Пусть я написал программу: """
  bello!
  poop a Papaya naidu!
  guoleila (a) naidu!
  tulalilloo ti amo (a) naidu!
  """
  Когда я ввожу в консоли: """
  10
  """
  Тогда я увижу в консоли: """
  Введите число: Result: 10
  """
```

