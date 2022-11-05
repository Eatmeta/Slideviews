using System;
using System.Collections.Generic;

namespace linq_slideviews
{
    public static class ExtensionsTask
    {
        /// <summary>
        /// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
        /// Медиана списка из четного количества элементов — это среднее арифметическое 
        /// двух серединных элементов списка после сортировки.
        /// </summary>
        /// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
        public static double Median(this IEnumerable<double> items)
        {
            var list = new List<double>();
            var counter = 0;
            
            foreach (var item in items)
            {
                if (counter == 0)
                    list.Add(item);
                else
                    AddToListAscending(list, item);
                counter++;
            }
            
            if (list.Count == 0)
                throw new InvalidOperationException();
            return list.Count % 2 != 0 
                ? list[list.Count / 2] 
                : (list[(list.Count / 2) - 1] + list[list.Count / 2]) / 2;
        }

        private static void AddToListAscending(List<double> list, double item)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] > item)
                {
                    list.Insert(i, item);
                    break;
                }
                if (i == list.Count - 1)
                {
                    list.Add(item);
                    break;
                }
            }
        }

        /// <returns>
        /// Возвращает последовательность, состоящую из пар соседних элементов.
        /// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
        /// </returns>
        public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
        {
            var isFirst = true;
            T temp = default;
            foreach (var item in items)
            {
                if (isFirst)
                    isFirst = false;
                else
                    yield return new Tuple<T, T>(temp, item);
                temp = item;
            }
        }
    }
}