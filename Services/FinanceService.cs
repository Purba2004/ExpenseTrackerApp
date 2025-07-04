namespace ExpenseTrackerApp.Services
{
    public class FinanceService
    {
        public void AddSalary(string email)
        {
            Console.Write("Enter source: ");
            string source = Console.ReadLine();
            Console.Write("Enter amount: ");
            double amount = double.Parse(Console.ReadLine());

            string entry = $"SALARY|{DateTime.Now}|{source}|{amount}";
            File.AppendAllLines($"Database/{email}_data.txt", new[] { entry });
            Console.WriteLine("Salary added.");
        }

        public void AddExpense(string email)
        {
            Console.Write("Enter amount: ");
            double amount = double.Parse(Console.ReadLine());

            Console.WriteLine("Select category:");
            Console.WriteLine("1. Food\n2. Travel\n3. Friend\n4. Other");
            string category = Console.ReadLine() switch
            {
                "1" => "Food",
                "2" => "Travel",
                "3" => "Friend",
                _ => "Other"
            };

            string entry = $"EXPENSE|{DateTime.Now}|{category}|{amount}";
            File.AppendAllLines($"Database/{email}_data.txt", new[] { entry });

            Console.WriteLine("Expense added.");
        }

        public void ShowNetBalance(string email)
        {
            var lines = File.ReadAllLines($"Database/{email}_data.txt");

            double salary = 0, expense = 0;
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts[0] == "SALARY")
                    salary += double.Parse(parts[3]);
                else if (parts[0] == "EXPENSE")
                    expense += double.Parse(parts[3]);
            }

            Console.WriteLine($"Total Salary: ₹{salary}");
            Console.WriteLine($"Total Expense: ₹{expense}");
            Console.WriteLine($"Net Balance: ₹{salary - expense}");
        }
    }
}
