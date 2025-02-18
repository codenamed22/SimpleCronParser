using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronParser
{
    /// <summary>
    /// Represents a single cron field (minute, hour, etc.) with its allowed range and expression.
    /// </summary>
    public class CronField
    {
        public string Name { get; }
        public int MinValue { get; }
        public int MaxValue { get; }
        public string Expression { get; }

        public CronField(string name, int minValue, int maxValue, string expression)
        {
            Name = name;
            MinValue = minValue;
            MaxValue = maxValue;
            Expression = expression;
        }

        /// <summary>
        /// Expands the field expression into a sorted list of allowed integer values.
        /// </summary>
        /// <returns>List of allowed values.</returns>
        public List<int> Expand()
        {
            var results = new HashSet<int>();

            // The expression may contain comma-separated segments.
            var segments = Expression.Split(',');
            foreach (var segment in segments)
            {
                var subResults = ExpandSegment(segment.Trim());
                foreach (var val in subResults)
                {
                    if (val < MinValue || val > MaxValue)
                    {
                        throw new ArgumentException($"Value {val} out of range for {Name}");
                    }
                    results.Add(val);
                }
            }

            var sortedResults = results.ToList();
            sortedResults.Sort();
            return sortedResults;
        }

        /// <summary>
        /// Expands an individual segment which may be a single value, a range, or a stepped range.
        /// </summary>
        private IEnumerable<int> ExpandSegment(string segment)
        {
            // Handle step expressions (e.g. "*/15" or "1-10/2")
            if (segment.Contains("/"))
            {
                var parts = segment.Split('/');
                if (parts.Length != 2)
                    throw new ArgumentException($"Invalid segment: {segment}");

                string basePart = parts[0];
                if (!int.TryParse(parts[1], out int step) || step <= 0)
                    throw new ArgumentException($"Invalid step value in segment: {segment}");

                int rangeStart, rangeEnd;
                if (basePart == "*")
                {
                    rangeStart = MinValue;
                    rangeEnd = MaxValue;
                }
                else if (basePart.Contains("-"))
                {
                    var rangeParts = basePart.Split('-');
                    if (rangeParts.Length != 2)
                        throw new ArgumentException($"Invalid range in segment: {segment}");

                    rangeStart = int.Parse(rangeParts[0]);
                    rangeEnd = int.Parse(rangeParts[1]);
                }
                else
                {
                    // Handle the uncommon case: single value with a step.
                    rangeStart = int.Parse(basePart);
                    rangeEnd = MaxValue;
                }

                for (int i = rangeStart; i <= rangeEnd; i += step)
                {
                    yield return i;
                }
            }
            else if (segment == "*")
            {
                // Asterisk means every value in the range.
                for (int i = MinValue; i <= MaxValue; i++)
                {
                    yield return i;
                }
            }
            else if (segment.Contains("-"))
            {
                // A range without a step, e.g. "1-5"
                var rangeParts = segment.Split('-');
                if (rangeParts.Length != 2)
                    throw new ArgumentException($"Invalid range segment: {segment}");

                int start = int.Parse(rangeParts[0]);
                int end = int.Parse(rangeParts[1]);
                for (int i = start; i <= end; i++)
                {
                    yield return i;
                }
            }
            else
            {
                // A single number.
                yield return int.Parse(segment);
            }
        }
    }
}
