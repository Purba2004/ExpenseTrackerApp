using System.Data.SqlClient;

namespace ExpenseTrackerApp.Services
{
    public class DatabaseInitializer
    {
        private readonly string masterConnection = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;";

        public void InitializeDatabase()
        {
            // Step 1: Create Database
            using (SqlConnection conn = new SqlConnection(masterConnection))
            {
                conn.Open();
                string createDbQuery = "IF DB_ID('ExpenseTrackerDB') IS NULL CREATE DATABASE ExpenseTrackerDB";
                SqlCommand cmd = new SqlCommand(createDbQuery, conn);
                cmd.ExecuteNonQuery();
            }

            // Step 2: Create Tables
            string dbConnection = @"Server=(localdb)\MSSQLLocalDB;Database=ExpenseTrackerDB;Integrated Security=true;";
            using (SqlConnection conn = new SqlConnection(dbConnection))
            {
                conn.Open();

                string createTables = @"
                IF OBJECT_ID('Users') IS NULL
                CREATE TABLE Users (
                    Id INT IDENTITY PRIMARY KEY,
                    Name NVARCHAR(100),
                    Email NVARCHAR(100) UNIQUE,
                    Password NVARCHAR(100)
                );

                IF OBJECT_ID('Salaries') IS NULL
                CREATE TABLE Salaries (
                    Id INT IDENTITY PRIMARY KEY,
                    UserId INT FOREIGN KEY REFERENCES Users(Id),
                    Date DATE,
                    Source NVARCHAR(100),
                    Amount DECIMAL(10,2)
                );

                IF OBJECT_ID('Expenses') IS NULL
                CREATE TABLE Expenses (
                    Id INT IDENTITY PRIMARY KEY,
                    UserId INT FOREIGN KEY REFERENCES Users(Id),
                    Date DATE,
                    Category NVARCHAR(50),
                    Amount DECIMAL(10,2)
                );
                ";

                SqlCommand cmd = new SqlCommand(createTables, conn);
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("✅ Database and tables initialized successfully.");
        }
    }
}
