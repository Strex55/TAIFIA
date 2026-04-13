# Список тестов для Parser

## Выражения (ParseExpressionTest)

### Числа и базовые операции
- [x] `1` → `1`
- [x] `10 melomo 5 flavuk 2` → `13`
- [x] `flavuk 10 melomo 5 flavuk flavuk 5` → `-10`
- [x] `10 dibotada 5 poopaye 2` → `25`
- [x] `10 pado 2` → `0`
- [x] `10 pado 3` → `1`
- [x] `1.128 melomo 8 flavuk 7.5` → `2` (округление до int)
- [x] `2 beedo 5` → `32`
- [x] `(2 melomo 3) poopaye 10` → `0` (округление до int)
- [x] `(flavuk 2) beedo 10` → `1024`
- [x] `flavuk 2 beedo 10` → `-1024`

### Логические операции
- [x] `da` → `1`
- [x] `no` → `0`
- [x] `5 con 5` → `1`
- [x] `5 nocon 10` → `1`
- [x] `5 la 10` → `1`
- [x] `5 la con 5` → `1`
- [x] `10 looka too 5` → `1`
- [x] `10 looka too con 10` → `1`
- [x] `da tropa no` → `0`
- [x] `da bo-ca no` → `1`
- [x] `makoroni da` → `0`
- [x] `makoroni no` → `1`

### Приоритет операторов
- [x] `2 melomo 3 dibotada 4` → `14`
- [x] `2 dibotada 3 beedo 2` → `18`
- [x] `(2 melomo 3) dibotada 4` → `20`
- [x] `flavuk 5 dibotada 3` → `-15`
- [x] `da tropa no bo-ca da` → `1`

### Функции
- [x] `muak(flavuk 15)` → `15`
- [x] `miniboss(5, 4)` → `4`
- [x] `bigboss(5, 4)` → `5`
- [x] `miniboss(bigboss(1, 5), miniboss(10, 6))` → `5`

---

## Программы верхнего уровня (ParseProgramTest)

### Минимальная программа
- [x] Минимальная программа с выводом: `bello!` + `tulalilloo ti amo (1) naidu!` → `[1]`

### Константы и переменные
- [x] Константы и переменные: `trusela pi Papaya 3.14` + `poop x Papaya` + `x lumai 5` + вывод → `[5]`
- [x] Константы `belloPi` и `belloE`: объявление без вывода → `[]`

### Ввод/вывод
- [x] Ввод и вывод: `guoleila (x)` + `tulalilloo ti amo (x melomo 1)` → `[6]` (при вводе `5`)

---

## Ввод/вывод через FakeEnvironment (ExecutesIoWithFakeEnvironment)

- [x] Ввод одного числа и вывод: ввод `5` → вывод `[6]`
- [x] Ввод двух чисел и вывод суммы: ввод `[10, 20]` → вывод `[30]`
- [x] Ввод, вычисление и вывод: ввод `5` → `x lumai x dibotada x` → вывод `[25]`

---

## Обработка ошибок (ParseErrorTest)

### Ошибки выражений
- [x] Незакрытая скобка: `(2 melomo 3`
- [x] Лишняя закрывающая скобка: `2 melomo 3)`
- [x] Неправильный порядок скобок: `)2 melomo 3(`
- [x] Отсутствие операнда: `2 melomo`
- [x] Пустые скобки: `()`
- [x] Незакрытый вызов функции: `muak(5`
- [x] Неправильный вызов функции: `muak 5)`
- [x] Пустые аргументы: `muak(,)`
- [x] Деление на ноль: `10 poopaye 0`
- [x] Остаток от деления на ноль: `10 pado 0`
- [x] Неизвестная переменная: `x`
- [x] Неизвестная переменная в выражении: `x melomo 5`
- [x] Неподдержанный строковый литерал: `!Hello!`

### Ошибки программ
- [x] Отсутствие `bello!`: программа без старта
- [x] Пропущенный `naidu!` в объявлении переменной: `poop x Papaya`
- [x] Неконстантное значение в `trusela`: `trusela pi Papaya x naidu!`
- [x] Пропущенный `naidu!` в объявлении: `poop x Papaya` без `naidu!`
- [x] Пропущенный `naidu!` в присваивании: `x lumai 5` без `naidu!`
- [x] Пропущенный `naidu!` в выводе: `tulalilloo ti amo (x)` без `naidu!`

---

## Примечания

- Все тесты используют `[Theory]` и `[MemberData]` для параметризации
- Тесты выражений проверяют результат через `EvaluateExpression()` (возвращает `int`)
- Тесты программ проверяют результаты через `FakeEnvironment.Results` (список `decimal`)
- Тесты ошибок проверяют выброс исключений `InvalidOperationException` или других соответствующих исключений
