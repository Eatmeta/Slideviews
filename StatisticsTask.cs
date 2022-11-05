using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
    public class StatisticsTask
    {
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            return visits.Count == 0
                ? 0.0
                : visits.All(visit => visit.SlideType != slideType)
                    ? 0.0
                    : visits
                        .OrderBy(visit => visit.DateTime)
                        .ToLookup(visit => visit.UserId, visit => new {visit.SlideType, visit.DateTime})
                        .ToLookup(visitUser => visitUser.Key, visitUser => visitUser.Bigrams())
                        .SelectMany(grouping =>
                            grouping.SelectMany(enumerable =>
                                enumerable.Where(tuple => tuple.Item1.SlideType == slideType)
                                    .Select(tuple => (tuple.Item2.DateTime - tuple.Item1.DateTime).TotalMinutes)))
                        .Where(minutes => minutes >= 1 && minutes <= 120)
                        .ToList()
                        .Median();
        }
    }
}