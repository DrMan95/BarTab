using Bar.WebApi.Hubs;
using BarBillHolderLibrary;
using BarBillHolderLibrary.Database;
using BarBillHolderLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IHubContext<BarHub> _hub;

        public RegisterController(IHubContext<BarHub> hub)
        {
            _hub = hub;
        }
        // GET: api/register
        [HttpGet]
        public ActionResult<RegisterDto> GetRegister()
        {
            if (BarBillHolderLibrary.Models.Bar.register == null)
                BarBillHolderLibrary.Models.Bar.register = new Register();

            decimal pending = 0m;

            if (BarBillHolderLibrary.Models.Bar.customers != null)
            {
                foreach (var customer in BarBillHolderLibrary.Models.Bar.customers)
                {
                    if (customer.bill != null)
                    {
                        pending += customer.bill.total;
                    }
                }
            }

            if (BarBillHolderLibrary.Models.Bar.tables != null)
            {
                foreach (var table in BarBillHolderLibrary.Models.Bar.tables)
                {
                    if (table.bill != null)
                    {
                        pending += table.bill.total;
                    }
                }
            }

            var dto = new RegisterDto(
                Cash: BarBillHolderLibrary.Models.Bar.register.cash,
                Card: BarBillHolderLibrary.Models.Bar.register.card,
                Pending: pending,
                Tips: BarBillHolderLibrary.Models.Bar.register.tips
            );

            return Ok(dto);
        }

        [HttpPost("close")]
        public async Task<IActionResult> CloseRegister()
        {
            if (BarBillHolderLibrary.Models.Bar.register == null)
                BarBillHolderLibrary.Models.Bar.register = new Register();

            // Recompute pending (same logic as GET)
            decimal pending = 0m;

            if (BarBillHolderLibrary.Models.Bar.customers != null)
            {
                foreach (var customer in BarBillHolderLibrary.Models.Bar.customers)
                {
                    if (customer.bill != null)
                        pending += customer.bill.total;
                }
            }

            if (BarBillHolderLibrary.Models.Bar.tables != null)
            {
                foreach (var table in BarBillHolderLibrary.Models.Bar.tables)
                {
                    if (table.bill != null)
                        pending += table.bill.total;
                }
            }

            if (pending != 0m)
                return BadRequest("Cannot close register: pending is not zero.");

            // Reset totals
            BarBillHolderLibrary.Models.Bar.register.cash = 0m;
            BarBillHolderLibrary.Models.Bar.register.card = 0m;
            BarBillHolderLibrary.Models.Bar.register.tips = 0m;

            // Persist to your bar JSON file
            await FileProcessor.SaveBarInstanceAsync();
            await _hub.Clients.All.SendAsync("RefreshAll");
            return NoContent();
        }
    }

    public record RegisterDto(
        decimal Cash,
        decimal Card,
        decimal Pending,
        decimal Tips
    );
}
