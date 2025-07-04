using System;
using System.Collections.Generic;
using System.IO;
using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Services
{
    public class AdminService
    {
        private readonly string userFile = "Database/users.txt";

        public bool Login(string username, string password)
        {
            string adminFile = "Database/AdminPassword.txt";
            if (!File.Exists(adminFile))
                File.WriteAllText(adminFile, "admin|admin123");

            var lines = File.ReadAllLines(adminFile);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts[0] == username && parts[1] == password)
                    return true;
            }
            return false;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            if (!File.Exists(userFile))
            {
                Console.WriteLine($"DEBUG: File not found at {Path.GetFullPath(userFile)}");
                return users;
            }

            var lines = File.ReadAllLines(userFile);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                if (parts.Length < 3) continue;

                users.Add(new User
                {
                    Name = parts[0],
                    Email = parts[1],
                    Password = parts[2]
                });
            }

            return users;
        }


        public void ViewUserData(string email)
        {
            string filePath = $"Database/{email}_data.txt";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No data found for this user.");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            Console.WriteLine($"\nTransactions for {email}:");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }
    }
}