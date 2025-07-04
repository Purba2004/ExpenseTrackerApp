using ExpenseTrackerApp.Models;
using ExpenseTrackerApp.Services;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Initialize Database
            var dbInit = new DatabaseInitializer();
            dbInit.InitializeDatabase();

            // Step 2: Initialize Services
            var userService = new UserService();
            var adminService = new AdminService();
            var financeService = new FinanceService();
            var calendarService = new CalendarService();

            while (true)
            {
                Console.WriteLine("\n=== Expense Tracker Main Menu ===");
                Console.WriteLine("1. Admin Login");
                Console.WriteLine("2. User Login/Register");
                Console.WriteLine("Type 'exit' to close the app.");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine()?.Trim().ToLower();

                if (choice == "exit")
                    break;

                if (choice == "1")
                {
                    Console.Write("Admin Username: ");
                    string username = Console.ReadLine();
                    Console.Write("Password: ");
                    string password = Console.ReadLine();

                    if (adminService.Login(username, password))
                    {
                        Console.WriteLine("\nAdmin Access Granted");

                        var users = adminService.GetAllUsers();
                        if (users.Count == 0)
                        {
                            Console.WriteLine("No registered users found.");
                            continue;
                        }

                        Console.WriteLine("\n=== Registered Users ===");
                        for (int i = 0; i < users.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {users[i].Name} ({users[i].Email})");
                        }

                        while (true)
                        {
                            Console.Write("\nSelect a user number to view data (or type 'back'): ");
                            string input = Console.ReadLine()?.Trim().ToLower();

                            if (input == "back")
                                break;

                            if (int.TryParse(input, out int selected) && selected >= 1 && selected <= users.Count)
                            {
                                string selectedEmail = users[selected - 1].Email;
                                adminService.ViewUserData(selectedEmail);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid selection. Please try again.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid admin credentials.");
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("\n1. Register");
                    Console.WriteLine("2. Login");
                    Console.Write("Choose an option: ");
                    string opt = Console.ReadLine();

                    if (opt == "1")
                    {
                        Console.Write("Name: ");
                        string name = Console.ReadLine();
                        Console.Write("Email: ");
                        string email = Console.ReadLine();
                        Console.Write("Password: ");
                        string password = Console.ReadLine();

                        var user = new User { Name = name, Email = email, Password = password };
                        if (userService.RegisterUser(user))
                            Console.WriteLine("Registered successfully.");
                        else
                            Console.WriteLine("Email already exists.");
                    }

                    Console.Write("Email: ");
                    string loginEmail = Console.ReadLine();
                    Console.Write("Password: ");
                    string loginPass = Console.ReadLine();

                    var currentUser = userService.LoginUser(loginEmail, loginPass);
                    if (currentUser == null)
                    {
                        Console.WriteLine("Login failed.");
                        continue;
                    }

                    Console.WriteLine($"\nWelcome {currentUser.Name}!");

                    bool runUserSession = true;
                    while (runUserSession)
                    {
                        Console.WriteLine("\n1. Add Salary");
                        Console.WriteLine("2. Add Expense");
                        Console.WriteLine("3. Show Net Balance");
                        Console.WriteLine("4. View Calendar");
                        Console.WriteLine("5. Logout");
                        Console.Write("Choose an action: ");

                        switch (Console.ReadLine())
                        {
                            case "1":
                                financeService.AddSalary(currentUser.Email);
                                break;
                            case "2":
                                financeService.AddExpense(currentUser.Email);
                                break;
                            case "3":
                                financeService.ShowNetBalance(currentUser.Email);
                                break;
                            case "4":
                                calendarService.ShowCalendarWithMarks(currentUser.Email);
                                break;
                            case "5":
                                runUserSession = false;
                                break;
                            default:
                                Console.WriteLine("Invalid choice.");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option. Try again.");
                }
            }

            Console.WriteLine("\nExiting... Thank you for using the Expense Tracker.");
        }
    }
}