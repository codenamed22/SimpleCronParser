namespace CronParser
{
    /// <summary>
    /// Main program for the Cron Parser application.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Expect exactly one argument containing the full cron string.
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: CronParserApp \"<cron_expression>\"");
                return;
            }

            try
            {
                var cronExpression = CronParser.Parse(args[0]);
                Console.WriteLine(cronExpression.GetExpandedOutput());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
