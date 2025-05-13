using OfficeOpenXml;
using ExpenseTrackerApp.Models;
using System.IO;
using System.Collections.Generic;
using System;

namespace ExpenseTrackerApp.Services
{
    public class ExcelService
    {
        private readonly string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "Database");
        private readonly string usersFile = "users.xlsx";

        public ExcelService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Ensure database directory exists
            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            // Create users.xlsx if it doesn't exist
            string usersFilePath = Path.Combine(dbPath, usersFile);
            if (!File.Exists(usersFilePath))
            {
                using var package = new ExcelPackage(new FileInfo(usersFilePath));
                var sheet = package.Workbook.Worksheets.Add("Users");
                sheet.Cells[1, 1].Value = "Username";
                sheet.Cells[1, 2].Value = "Password";
                package.Save();
            }
        }

        public bool RegisterUser(User user)
        {
            string path = Path.Combine(dbPath, usersFile);
            using var package = new ExcelPackage(new FileInfo(path));
            var sheet = package.Workbook.Worksheets["Users"];

            // Sanitize input
            string newUsername = user.Username?.Trim().ToLower();
            string newPassword = user.Password?.Trim();

            if (string.IsNullOrEmpty(newUsername) || string.IsNullOrEmpty(newPassword))
                return false;

            int lastRow = sheet.Dimension?.End.Row ?? 1;

            // Check for existing username
            for (int i = 2; i <= lastRow; i++)
            {
                string existingUsername = sheet.Cells[i, 1].Text?.Trim().ToLower();
                if (existingUsername == newUsername)
                    return false; // User already exists
            }

            // Add new user
            int newRow = lastRow + 1;
            sheet.Cells[newRow, 1].Value = newUsername;
            sheet.Cells[newRow, 2].Value = newPassword;
            package.Save();

            CreateUserFile(newUsername);
            return true;
        }

        public bool ValidateUser(User user)
        {
            string path = Path.Combine(dbPath, usersFile);
            if (!File.Exists(path)) return false;

            using var package = new ExcelPackage(new FileInfo(path));
            var sheet = package.Workbook.Worksheets["Users"];

            string inputUsername = user.Username?.Trim().ToLower();
            string inputPassword = user.Password?.Trim();

            for (int i = 2; i <= sheet.Dimension.End.Row; i++)
            {
                string existingUsername = sheet.Cells[i, 1].Text?.Trim().ToLower();
                string existingPassword = sheet.Cells[i, 2].Text?.Trim();

                if (existingUsername == inputUsername && existingPassword == inputPassword)
                    return true;
            }
            return false;
        }

        private void CreateUserFile(string username)
        {
            string path = Path.Combine(dbPath, $"{username}.xlsx");

            using var package = new ExcelPackage(new FileInfo(path));
            var sheet = package.Workbook.Worksheets.Add("Expenses");
            sheet.Cells[1, 1].Value = "Date";
            sheet.Cells[1, 2].Value = "Category";
            sheet.Cells[1, 3].Value = "Amount";
            sheet.Cells[1, 4].Value = "Type";
            sheet.Cells[1, 5].Value = "RemainingBalance";
            package.Save();
        }

        public void AddExpense(string username, Expense expense)
        {
            string path = Path.Combine(dbPath, $"{username}.xlsx");
            using var package = new ExcelPackage(new FileInfo(path));
            var sheet = package.Workbook.Worksheets["Expenses"];

            int lastRow = sheet.Dimension?.End.Row ?? 1;

            decimal lastBalance = lastRow == 1 ? 0 : Convert.ToDecimal(sheet.Cells[lastRow, 5].Text);
            decimal newBalance = expense.Type == "+" ? lastBalance + expense.Amount : lastBalance - expense.Amount;
            int newRow = lastRow + 1;

            sheet.Cells[newRow, 1].Value = expense.Date.ToString("yyyy-MM-dd");
            sheet.Cells[newRow, 2].Value = expense.Category;
            sheet.Cells[newRow, 3].Value = expense.Amount;
            sheet.Cells[newRow, 4].Value = expense.Type;
            sheet.Cells[newRow, 5].Value = newBalance;
            package.Save();
        }

        public List<Expense> GetExpenses(string username)
        {
            string path = Path.Combine(dbPath, $"{username}.xlsx");
            using var package = new ExcelPackage(new FileInfo(path));
            var sheet = package.Workbook.Worksheets["Expenses"];

            List<Expense> expenses = new();
            for (int i = 2; i <= sheet.Dimension.End.Row; i++)
            {
                expenses.Add(new Expense
                {
                    Date = DateTime.Parse(sheet.Cells[i, 1].Text),
                    Category = sheet.Cells[i, 2].Text,
                    Amount = decimal.Parse(sheet.Cells[i, 3].Text),
                    Type = sheet.Cells[i, 4].Text,
                    RemainingBalance = decimal.Parse(sheet.Cells[i, 5].Text)
                });
            }
            return expenses;
        }
    }
}
