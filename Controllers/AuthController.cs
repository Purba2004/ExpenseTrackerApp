using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerApp.Services;
using ExpenseTrackerApp.Models;

namespace ExpenseTrackerApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ExcelService _excelService;

        public AuthController(ExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] User user)
        {
            var success = _excelService.RegisterUser(user);
            if (success)
            {
                return Ok(new { success = true, message = "User  registered successfully." });
            }
            return BadRequest(new { success = false, message = "Username already exists." });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var validUser = _excelService.ValidateUser(user);
            if (validUser)
            {
                return Ok(new { success = true, message = "Login successful." });
            }
            return BadRequest(new { success = false, message = "Invalid username or password." });
        }
    }
}
