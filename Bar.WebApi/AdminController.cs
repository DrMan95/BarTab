using Microsoft.AspNetCore.Mvc;

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class AdminLoginRequest
        {
            public string Password { get; set; } = "";
        }

        [HttpPost("verify")]
        public IActionResult Verify([FromBody] AdminLoginRequest request)
        {
            var configuredPassword = _configuration["AdminSettings:Password"];

            if (string.IsNullOrWhiteSpace(configuredPassword))
                return StatusCode(500, "Admin password is not configured.");

            if (request == null || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Password is required.");

            if (request.Password != configuredPassword)
                return Unauthorized("Wrong password.");

            return Ok(new { success = true });
        }
    }
}