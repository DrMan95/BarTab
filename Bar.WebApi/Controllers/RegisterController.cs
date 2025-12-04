using Microsoft.AspNetCore.Mvc;
using BarBillHolderLibrary.Models;
using BarBillHolderLibrary;

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
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
    }

    public record RegisterDto(
        decimal Cash,
        decimal Card,
        decimal Pending,
        decimal Tips
    );
}
