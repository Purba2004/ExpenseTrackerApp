using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerApp.Models;
using ExpenseTrackerApp.Services;

namespace ExpenseTrackerApp.Controllers
{
    [Route("api/expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExcelService _excelService;

        public ExpenseController(ExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost("{username}")]
        public IActionResult AddExpense(string username, [FromBody] Expense expense)
        {
            try
            {
                _excelService.AddExpense(username, expense);
                return Ok(new { success = true, message = "Expense added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Failed to add expense.", error = ex.Message });
            }
        }

        [HttpGet("{username}")]
        public IActionResult GetExpenses(string username)
        {
            try
            {
                var expenses = _excelService.GetExpenses(username);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Failed to retrieve expenses.", error = ex.Message });
            }
        }
    }
}
