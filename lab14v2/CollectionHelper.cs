using System;
using System.Collections.Generic;

namespace Lab14v2
{
    /// <summary>
    /// Статичний узагальнений клас-утиліта для роботи з колекціями.
    /// Варіант 2: метод Map.
    /// </summary>
    /// <typeparam name="T">Тип елементів вхідної колекції.</typeparam>
    public static class CollectionHelper<T>
    {
        /// <summary>
        /// Перетворює кожен елемент колекції типу T на елемент типу TResult
        /// за допомогою делегата <paramref name="mapper"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Якщо collection або mapper дорівнює null (перевіряється одразу при виклику).
        /// </exception>
        public static IEnumerable<TResult> Map<TResult>(IEnumerable<T> collection, Func<T, TResult> mapper)
        {
            // Перевірка аргументів виконується тут, а не в ітераторі,
            // щоб помилка виникала одразу, а не під час першого перебору.
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return MapIterator(collection, mapper);
        }

        // Власна реалізація: проходимо колекцію і повертаємо результат mapper для кожного елемента.
        private static IEnumerable<TResult> MapIterator<TResult>(IEnumerable<T> collection, Func<T, TResult> mapper)
        {
            foreach (T item in collection)
            {
                yield return mapper(item);
            }
        }
    }
}
