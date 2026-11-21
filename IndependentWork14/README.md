# Самостійна робота №14: Dictionary<TKey, TValue> та HashSet<T>

Дисципліна: Об'єктно-орієнтоване програмування. Виконав: Висоцький Олександр, група ІПЗ-3/1.

Консольний застосунок демонструє використання `Dictionary` і `HashSet` у трьох сценаріях:

| Сценарій | Файл | Колекції |
|---|---|---|
| 1. Кеш метаданих файлів | `FileMetadataCache.cs` | `Dictionary<string, FileMetadata>` |
| 2. Унікальні теги статей | `TagManager.cs` | `Dictionary<string, HashSet<string>>` |
| 3. Пошуковий індекс (додатково) | `SearchIndex.cs` | `Dictionary<string, HashSet<int>>` |

Аналіз open-source прикладів, порівняльна таблиця та висновки лежать у [REPORT.md](REPORT.md).

## Запуск

```bash
dotnet run --project IndependentWork14
```

## Приклад виводу програми

```text
=== Сценарій 1: FileMetadataCache (Dictionary) ===
Додано 2 файли, Count = 2
Повторне додавання REPORT.DOCX (регістр не важливий): False
Contains("photo.png")  = True
Get("photo.png")       = photo.png | 2048000 байт | 2026-10-28 09:05 | hash=d4e5f6
Get("missing.txt")     = null
Remove("photo.png")    = True
Remove("photo.png")    = False (вдруге)
Count = 1

=== Сценарій 2: TagManager (Dictionary + HashSet) ===
Теги article-1: C#, collections, performance
Теги article-2: C#, performance, testing
Усі унікальні теги (UnionWith): C#, collections, performance, python, testing
Спільні теги 1 і 2 (IntersectWith): C#, performance
Лише в article-1 (ExceptWith): collections
Після RemoveTag(article-1, performance) спільні теги: C#

=== Сценарій 3: SearchIndex (Dictionary<string, HashSet<int>>) ===
Search("stores")                 -> [1, 2]
Search("dictionary")             -> [1, 3]
SearchMultiple("hashset","fast") -> [3]
SearchMultiple("dictionary","unique") -> []
Search("python")                 -> []
```
