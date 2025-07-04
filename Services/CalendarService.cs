namespace ExpenseTrackerApp.Services
{
    public class CalendarService
    {
        public void ShowCalendarWithMarks(string email)
        {
            Console.WriteLine("Enter month (1-12): ");
            int month = int.Parse(Console.ReadLine());
            int year = DateTime.Now.Year;

            var lines = File.ReadAllLines($"Database/{email}_data.txt");

            var markedDates = new HashSet<int>();
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                var date = DateTime.Parse(parts[1]);
                if (date.Month == month)
                    markedDates.Add(date.Day);
            }

            Console.WriteLine($"\nCalendar for {month}/{year}:");
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                if (markedDates.Contains(i))
                    Console.Write($"[{i:00}] ");
                else
                    Console.Write($"{i:00} ");
            }
            Console.WriteLine();
        }
    }
}
