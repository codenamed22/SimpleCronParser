namespace CronParser
{
    /// <summary>
    /// Parses the input cron string.
    /// </summary>
    public static class CronParser
    {
        /// <summary>
        /// Parses an input string such as:
        /// "*/15 0 1,15 * 1-5 /usr/bin/find"
        /// </summary>
        public static CronExpression Parse(string input)
        {
            // Split input by whitespace. The first five tokens are the cron fields;
            // everything else is part of the command.
            var parts = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 6)
                throw new ArgumentException("Input does not contain enough fields.");

            string minute = parts[0];
            string hour = parts[1];
            string dayOfMonth = parts[2];
            string month = parts[3];
            string dayOfWeek = parts[4];
            string command = string.Join(" ", parts.Skip(5));

            return new CronExpression(minute, hour, dayOfMonth, month, dayOfWeek, command);
        }
    }
}
