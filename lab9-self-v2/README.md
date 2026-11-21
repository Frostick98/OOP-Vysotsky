# lab9-self-v2: делегати й лямбда-вирази, утиліти для колекцій

Дисципліна: Об'єктно-орієнтоване програмування, самостійна робота №15.
Виконав: Висоцький Олександр, група ІПЗ-3/1.

Статичний клас `CollectionUtils` (файл `CollectionUtils.cs`) містить чотири узагальнені методи на делегатах:

| Метод | Делегат | Аналог |
|---|---|---|
| `Map<TSource, TResult>` | `Func<TSource, TResult>` | LINQ `Select` |
| `MaxBy<T, TKey>` | `Func<T, TKey>`, де `TKey : IComparable<TKey>` | LINQ `MaxBy` |
| `Filter<T>` | `Predicate<T>` | LINQ `Where` |
| `ForEach<T>` | `Action<T>` | `List<T>.ForEach` |

Демонстрація в `Program.cs` працює з трьома колекціями (`List<Product>`, `List<int>`, `List<string>`), показує відкладене виконання, граничні випадки та порівняння з LINQ. Звіт: [REPORT.md](REPORT.md).

## Запуск

```bash
dotnet run --project lab9-self-v2
```

## Приклад виводу програми

```text
=== Колекція 1: List<Product> ===
Names: Laptop, Mouse, Keyboard, Monitor
Ціни зі знижкою 10%: 22500.0, 450.0, 1350.0, 7200.0
Most expensive: Laptop - 25000
Найдовша назва: Keyboard
Expensive products:
 - Laptop
 - Keyboard
 - Monitor
Laptop: 25000 грн
Mouse: 500 грн
Keyboard: 1500 грн
Monitor: 8000 грн

=== Колекція 2: List<int> ===
Квадрати: 9, 64, 4, 225, 49, 100, 81, 16
Парні: 8, -2, 10, 4
Додатні: 3, 8, 15, 7, 10, 4
Максимум: 15
Найдалі від нуля: 15
Сума через ForEach: 36

=== Колекція 3: List<string> ===
Верхній регістр: DELEGATE, LAMBDA, FUNC, ACTION, PREDICATE, LINQ
Довжини: delegate(8), lambda(6), Func(4), Action(6), Predicate(9), LINQ(4)
Містять літеру 'a': delegate, lambda, Action, Predicate
Найдовше слово: Predicate
Останнє за алфавітом: Predicate
[delegate] [lambda] [Func] [Action] [Predicate] [LINQ] 

=== Відкладене виконання Map і Filter ===
Map викликано, але mapper ще не виконувався.
Починаємо перебір:
  mapper викликано для 1
  отримано 2
  mapper викликано для 2
  отримано 4
  mapper викликано для 3
  отримано 6

=== Граничні випадки ===
MaxBy для порожньої колекції: InvalidOperationException: Колекція не містить жодного елемента.
Filter з null-предикатом: ArgumentNullException (параметр predicate)

=== Порівняння з LINQ ===
Map    == Select : True
Filter == Where  : True
MaxBy  == MaxBy  : True
```
