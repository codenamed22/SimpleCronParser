namespace CronParser
{
    /// <summary>
    /// Represents a full cron expression with five fields and a command.
    /// </summary>
    public class CronExpression
    {
        public CronField Minute { get; }
        public CronField Hour { get; }
        public CronField DayOfMonth { get; }
        public CronField Month { get; }
        public CronField DayOfWeek { get; }
        public string Command { get; }

        public CronExpression(string minute, string hour, string dayOfMonth, string month, string dayOfWeek, string command)
        {
            Minute = new CronField("minute", 0, 59, minute);
            Hour = new CronField("hour", 0, 23, hour);
            DayOfMonth = new CronField("day of month", 1, 31, dayOfMonth);
            Month = new CronField("month", 1, 12, month);
            DayOfWeek = new CronField("day of week", 0, 6, dayOfWeek);
            Command = command;
        }

        /// <summary>
        /// Produces a formatted output with each field padded to 14 columns.
        /// </summary>
        /// <summary>
        /// Produces a formatted output with each field padded to 14 columns.
        /// </summary>
        public string GetExpandedOutput()
        {
            var output = new List<string>
            {
                FormatLine(Minute.Name, Minute.Expand()),
                FormatLine(Hour.Name, Hour.Expand()),
                FormatLine(DayOfMonth.Name, DayOfMonth.Expand()),
                FormatLine(Month.Name, Month.Expand()),
                FormatLine(DayOfWeek.Name, DayOfWeek.Expand()),
                $"{PadField("command")}{Command}"
            };

            return string.Join(Environment.NewLine, output);
        }


        private string FormatLine(string fieldName, List<int> values)
        {
            return $"{PadField(fieldName)}{string.Join(" ", values)}";
        }

        private string PadField(string fieldName)
        {
            return fieldName.PadRight(14);
        }
    }
}
