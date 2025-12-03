using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BarBillHolderLibrary;          // Customer, Item
using BarBillHolderLibrary.Models;   // Bar, Bill
using BarBillHolderLibrary.Database; // FileProcessor

using BarState = BarBillHolderLibrary.Models.Bar;

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        // GET: api/customers
        [HttpGet]
        public ActionResult<IEnumerable<CustomerSummaryDto>> GetAll()
        {
            if (BarState.customers == null)
                return Ok(Enumerable.Empty<CustomerSummaryDto>());

            var result = BarState.customers.Select(c => new CustomerSummaryDto(
                Name: c.name,
                Total: c.bill?.total ?? 0m,
                PendingItems: c.bill?.items?.Count(i => i.status == Item.Status.UNDONE) ?? 0
            ));

            return Ok(result);
        }

        // GET: api/customers/Bar%201
        [HttpGet("{name}")]
        public ActionResult<CustomerDetailDto> GetOne(string name)
        {
            var customer = FindCustomerByName(name);
            if (customer == null)
                return NotFound($"Customer '{name}' not found.");

            return Ok(MapDetail(customer));
        }

        // POST: api/customers
        // No body needed – auto-generate "Bar 1", "Bar 2", ...
        [HttpPost]
        public async Task<ActionResult<CustomerSummaryDto>> Create()
        {
            if (BarState.customers == null)
                BarState.customers = new List<Customer>();

            // Find next "Bar N" that doesn't exist yet
            int index = 1;
            string name;
            do
            {
                name = $"Bar {index}";
                index++;
            } while (BarState.customers.Any(c =>
                string.Equals(c.name, name, System.StringComparison.OrdinalIgnoreCase)));

            var newCustomer = new Customer(name);
            BarState.customers.Add(newCustomer);

            await FileProcessor.SaveBarInstanceAsync();

            var dto = new CustomerSummaryDto(
                Name: newCustomer.name,
                Total: newCustomer.bill?.total ?? 0m,
                PendingItems: 0
            );

            return CreatedAtAction(nameof(GetOne), new { name = dto.Name }, dto);
        }

        // ---------- helpers ----------

        private static Customer? FindCustomerByName(string name)
        {
            if (BarState.customers == null) return null;

            return BarState.customers.FirstOrDefault(c =>
                string.Equals(c.name, name, System.StringComparison.OrdinalIgnoreCase));
        }

        private static CustomerDetailDto MapDetail(Customer c)
        {
            var bill = c.bill ?? new Bill();

            return new CustomerDetailDto(
                Name: c.name,
                Total: bill.total,
                Items: bill.items?.Select(i => new CustomerBillItemDto(
                    Name: i.name,
                    Category: i.category,
                    Price: i.price,
                    Status: i.status.ToString()
                )) ?? Enumerable.Empty<CustomerBillItemDto>()
            );
        }
    }

    // List view
    public record CustomerSummaryDto(
        string Name,
        decimal Total,
        int PendingItems
    );

    // Detailed view
    public record CustomerDetailDto(
        string Name,
        decimal Total,
        IEnumerable<CustomerBillItemDto> Items
    );

    public record CustomerBillItemDto(
        string Name,
        string Category,
        decimal Price,
        string Status
    );
}

