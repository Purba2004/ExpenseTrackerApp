<<<<<<< HEAD
﻿namespace ExpenseTrackerApp.Models
{
    public class Expense
    {
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
=======
namespace ExpenseTrackerApp.Models
{
    public class Expense
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // "+" or "-"
        public DateTime Date { get; set; }
        public decimal RemainingBalance { get; set; }
>>>>>>> d27af8e9e90fae3fd3d028adacdcc30ad86cea5e
    }
}
