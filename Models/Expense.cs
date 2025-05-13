namespace ExpenseTrackerApp.Models
{
    public class Expense
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // "+" or "-"
        public DateTime Date { get; set; }
        public decimal RemainingBalance { get; set; }
    }
}
