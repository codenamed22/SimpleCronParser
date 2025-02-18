namespace CronParser.Tests
{
    public class CronParserTests
    {
        [Fact]
        public void TestMinuteExpansion_Asterisk()
        {
            var field = new CronField("minute", 0, 59, "*");
            var expected = Enumerable.Range(0, 60).ToList();
            var result = field.Expand();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestMinuteExpansion_Step()
        {
            var field = new CronField("minute", 0, 59, "*/15");
            var expected = new int[] { 0, 15, 30, 45 };
            var result = field.Expand();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestHourExpansion_SingleValue()
        {
            var field = new CronField("hour", 0, 23, "0");
            var expected = new int[] { 0 };
            var result = field.Expand();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestDayOfMonthExpansion_List()
        {
            var field = new CronField("day of month", 1, 31, "1,15");
            var expected = new int[] { 1, 15 };
            var result = field.Expand();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestMonthExpansion_Asterisk()
        {
            var field = new CronField("month", 1, 12, "*");
            var expected = Enumerable.Range(1, 12).ToList();
            var result = field.Expand();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestDayOfWeekExpansion_Range()
        {
            var field = new CronField("day of week", 0, 6, "1-5");
            var expected = new int[] { 1, 2, 3, 4, 5 };
            var result = field.Expand();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestCronExpressionParsingAndOutput()
        {
            string input = "*/15 0 1,15 * 1-5 /usr/bin/find";
            var cronExpression = CronParser.Parse(input);
            var output = cronExpression.GetExpandedOutput();
            var expected = "minute        0 15 30 45" + Environment.NewLine +
                           "hour          0" + Environment.NewLine +
                           "day of month  1 15" + Environment.NewLine +
                           "month         1 2 3 4 5 6 7 8 9 10 11 12" + Environment.NewLine +
                           "day of week   1 2 3 4 5" + Environment.NewLine +
                           "command       /usr/bin/find";
            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestCronExpressionParsingAndOutput_Complex()
        {
            string input = "0 12 1-15/2 1,6 0-3,5 /usr/bin/backup";
            var cronExpression = CronParser.Parse(input);
            var output = cronExpression.GetExpandedOutput();
            var expected = "minute        0" + Environment.NewLine +
                           "hour          12" + Environment.NewLine +
                           "day of month  1 3 5 7 9 11 13 15" + Environment.NewLine +
                           "month         1 6" + Environment.NewLine +
                           "day of week   0 1 2 3 5" + Environment.NewLine +
                           "command       /usr/bin/backup";
            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestCronExpressionParsingAndOutput_Mixed()
        {
            string input = "5,10,15 0-12/2 * 1,3,5 0-6 /usr/bin/script";
            var cronExpression = CronParser.Parse(input);
            var output = cronExpression.GetExpandedOutput();
            var expected = "minute        5 10 15" + Environment.NewLine +
                           "hour          0 2 4 6 8 10 12" + Environment.NewLine +
                           "day of month  1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31" + Environment.NewLine +
                           "month         1 3 5" + Environment.NewLine +
                           "day of week   0 1 2 3 4 5 6" + Environment.NewLine +
                           "command       /usr/bin/script";
            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestCronExpressionParsingAndOutput_AllAsterisks()
        {
            string input = "* * * * * /usr/bin/anytime";
            var cronExpression = CronParser.Parse(input);
            var output = cronExpression.GetExpandedOutput();
            var expected = "minute        0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59" + Environment.NewLine +
                           "hour          0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23" + Environment.NewLine +
                           "day of month  1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31" + Environment.NewLine +
                           "month         1 2 3 4 5 6 7 8 9 10 11 12" + Environment.NewLine +
                           "day of week   0 1 2 3 4 5 6" + Environment.NewLine +
                           "command       /usr/bin/anytime";
            Assert.Equal(expected, output);
        }
    }
}
