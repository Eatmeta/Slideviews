using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
    public class ParsingTask
    {
        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка заголовочная.</param>
        /// <returns>Словарь: ключ — идентификатор слайда, значение — информация о слайде</returns>
        /// <remarks>Метод должен пропускать некорректные строки, игнорируя их</remarks>
        public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
        {
            return lines
                .Skip(1)
                .Select(line =>
                {
                    try
                    {
                        return new SlideRecord(int.Parse(line.Split(';')[0]),
                            (SlideType) Enum.Parse(typeof(SlideType), line.Split(';')[1], true),
                            line.Split(';')[2]);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }).Where(x => x != null)
                .ToDictionary(x => x.SlideId, x => x);
        }

        /// <param name="lines">все строки файла, которые нужно распарсить. Первая строка — заголовочная.</param>
        /// <param name="slides">Словарь информации о слайдах по идентификатору слайда. 
        /// Такой словарь можно получить методом ParseSlideRecords</param>
        /// <returns>Список информации о посещениях</returns>
        /// <exception cref="FormatException">Если среди строк есть некорректные</exception>
        public static IEnumerable<VisitRecord> ParseVisitRecords(
            IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
        {
            return lines
                .Skip(1)
                .Select(line => new VisitRecord(
                    int.TryParse(line.Split(';')[0], out var userId)
                        ? userId
                        : throw new FormatException($"Wrong line [{line}]"),
                    int.TryParse(line.Split(';')[1], out var slideId)
                        ? slideId
                        : throw new FormatException($"Wrong line [{line}]"),
                    DateTime.TryParseExact(line.Split(';')[2] + " " + line.Split(';')[3], "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime)
                        ? dateTime
                        : throw new FormatException($"Wrong line [{line}]"),
                    Enum.TryParse(slides.Values.Any(x =>
                            x.SlideId == int.Parse(line.Split(';')[1]))
                            ? slides.Values.Where(z => z.SlideId == int.Parse(line.Split(';')[1]))
                                .Select(z => z.SlideType).Single().ToString()
                            : throw new FormatException($"Wrong line [{line}]")
                        , true, out SlideType slideType)
                        ? slideType
                        : throw new FormatException($"Wrong line [{line}]")
                ));
        }
    }
}
