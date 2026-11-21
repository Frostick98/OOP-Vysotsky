# InvertedIndexDemo: інвертований індекс і вимірювання продуктивності

Дисципліна: Об'єктно-орієнтоване програмування, самостійна робота №16.
Виконав: Висоцький Олександр, група ІПЗ-3/1.

Консольний застосунок з власною реалізацією інвертованого індексу `Dictionary<string, HashSet<int>>`:

| Файл | Призначення |
|---|---|
| `Document.cs` | Документ (Id, Title, Content) |
| `TextProcessor.cs` | `Tokenize` (Regex, нижній регістр, без пунктуації) та `RemoveStopWords` |
| `InvertedIndex.cs` | `AddDocument`, `SearchSingleWord`, `SearchMultipleWords` (AND, `IntersectWith`), `SearchAnyWord` (OR, `UnionWith`), додатково `SearchPhrase` |
| `TestData.cs` | П'ять документів із завдання та генерація великих наборів |
| `PerformanceTest.cs` | Порівняння лінійного пошуку з індексом |
| `Program.cs` | Демонстрація, вимірювання та інтерактивний пошук |

Опис реалізації, таблиця вимірювань і висновки: [REPORT.md](REPORT.md).

## Запуск

Для коректних вимірювань запускайте у конфігурації Release:

```bash
dotnet run --project InvertedIndexDemo -c Release
```

Після демонстрації і вимірювань програма запускає інтерактивний пошук (`exit` для виходу). Якщо вхід перенаправлено, цей крок пропускається.

## Приклад виводу програми

Числа у таблиці залежать від комп'ютера, співвідношення між лінійним пошуком та індексом залишаються схожими.

```text
Loaded 5 documents
Index contains 37 unique words

Query (AND): programming language
[1] Introduction to C#
  C# is a modern object-oriented programming language developed by Microsoft....
[2] Java Basics
  Java is a popular programming language known for its portability and performance....
[3] Python Overview
  Python is a high-level programming language with simple syntax....
Found 3 documents

Query (AND): c# async
[4] C# Advanced Features
  C# supports LINQ, async/await, and many modern programming paradigms....
Found 1 documents

Query (OR): python lisp java
[2] Java Basics
  Java is a popular programming language known for its portability and performance....
[3] Python Overview
  Python is a high-level programming language with simple syntax....
[5] Comparing Languages
  C#, Java, and Python are among the most popular programming languages today....
Found 3 documents

Query (SINGLE): portability
[2] Java Basics
  Java is a popular programming language known for its portability and performance....
Found 1 documents

Query (PHRASE): modern programming
[4] C# Advanced Features
  C# supports LINQ, async/await, and many modern programming paradigms....
Found 1 documents

Query (AND): modern programming (для порівняння з фразою)
[1] Introduction to C#
  C# is a modern object-oriented programming language developed by Microsoft....
[4] C# Advanced Features
  C# supports LINQ, async/await, and many modern programming paradigms....
Found 2 documents

Tokenize:        the, cat, is, on, the, mat, and, the, dog, is, in, the, house
RemoveStopWords: cat, mat, dog, house

=== Порівняння лінійного пошуку та інвертованого індексу ===
(час одного пошуку, середнє за багато повторів; результати обох підходів перевіряються на збіг)

| Документів | Слово | Знайдено | Лінійний, мкс | Індекс, мкс | Прискорення | Побудова індексу, мкс | Окупність, запитів |
|---:|---|---:|---:|---:|---:|---:|---:|
| 10 | programming | 10 | 33.9 | 3.00 | 11x | 134 | 5 |
| 10 | java | 4 | 47.1 | 1.88 | 25x | 125 | 3 |
| 10 | paradigms | 2 | 54.5 | 1.64 | 33x | 89 | 2 |
| 10 | quokka | 0 | 54.2 | 0.45 | 120x | 71 | 2 |
| 100 | programming | 100 | 216.9 | 16.34 | 13x | 816 | 5 |
| 100 | java | 40 | 426.7 | 6.13 | 70x | 1023 | 3 |
| 100 | paradigms | 20 | 406.5 | 6.85 | 59x | 959 | 3 |
| 100 | quokka | 0 | 410.0 | 0.90 | 458x | 1154 | 3 |
| 1000 | programming | 1000 | 1229.0 | 129.65 | 9x | 6006 | 6 |
| 1000 | java | 400 | 2194.4 | 50.22 | 44x | 6029 | 3 |
| 1000 | paradigms | 200 | 3345.8 | 5.26 | 636x | 6139 | 2 |
| 1000 | quokka | 0 | 3481.3 | 0.32 | 10875x | 4444 | 2 |
| 10000 | programming | 10000 | 11279.5 | 344.77 | 33x | 56712 | 6 |
| 10000 | java | 4000 | 19206.3 | 55.17 | 348x | 52061 | 3 |
| 10000 | paradigms | 2000 | 29989.1 | 36.96 | 811x | 48894 | 2 |
| 10000 | quokka | 0 | 31630.2 | 0.24 | 130435x | 50849 | 2 |

```

Приклад інтерактивного режиму:

```text
Enter search query (or 'exit'): programming language

--- Search Results (AND) ---
[1] Introduction to C#
  C# is a modern object-oriented programming language developed by Microsoft....
[2] Java Basics
  Java is a popular programming language known for its portability and performance....
[3] Python Overview
  Python is a high-level programming language with simple syntax....

Found 3 documents

Enter search query (or 'exit'): c# async

--- Search Results (AND) ---
[4] C# Advanced Features
  C# supports LINQ, async/await, and many modern programming paradigms....

Found 1 documents

Enter search query (or 'exit'): exit
```
