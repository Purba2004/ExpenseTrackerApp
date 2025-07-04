using Microsoft.EntityFrameworkCore;

using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
            : base(options) { }

        public DbSet<Expense> Expenses { get; set; }
    }
}
