using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Services
{
    public class UserService
    {
        private readonly string userFile = "Database/users.txt";

        public UserService()
        {
            if (!Directory.Exists("Database"))
                Directory.CreateDirectory("Database");

            if (!File.Exists(userFile))
                File.Create(userFile).Close();
        }

        public bool RegisterUser(User user)
        {
            if (IsUserExists(user.Email)) return false;

            string line = $"{user.Name}|{user.Email}|{user.Password}";
            File.AppendAllLines(userFile, new[] { line });

            // Create individual user file
            File.Create($"Database/{user.Email}_data.txt").Close();

            return true;
        }

        public User LoginUser(string email, string password)
        {
            var lines = File.ReadAllLines(userFile);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts[1] == email && parts[2] == password)
                {
                    return new User { Name = parts[0], Email = parts[1], Password = parts[2] };
                }
            }
            return null;
        }

        public bool IsUserExists(string email)
        {
            return File.ReadAllLines(userFile).Any(l => l.Split('|')[1] == email);
        }
    }
}
