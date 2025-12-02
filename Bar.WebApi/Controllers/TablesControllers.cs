using Microsoft.AspNetCore.Mvc;
using BarBillHolderLibrary.Models; // Bar, Table, Bill, Register
using BarBillHolderLibrary;        // Item, Customer

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        // GET: api/tables
        [HttpGet]
        public ActionResult<IEnumerable<TableDto>> GetAllTables()
        {
            if (BarBillHolderLibrary.Models.Bar.tables == null)
                return Ok(Enumerable.Empty<TableDto>());

            var result = BarBillHolderLibrary.Models.Bar.tables.Select(MapTableToDto).ToList();
            return Ok(result);
        }

        // GET: api/tables/3
        [HttpGet("{id:int}")]
        public ActionResult<TableDto> GetTable(int id)
        {
            var table = BarBillHolderLibrary.Models.Bar.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            return Ok(MapTableToDto(table));
        }

        // POST: api/tables/3/items
        // Body: { "name": "...", "category": "...", "price": 2.50 }
        [HttpPost("{id:int}/items")]
        public ActionResult<TableDto> AddItemToTable(int id, [FromBody] AddItemRequest request)
        {
            var table = BarBillHolderLibrary.Models.Bar.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (table.bill == null)
                table.bill = new Bill();

            table.open = true;

            var item = new Item(request.Name, request.Category, request.Price, Item.Status.UNDONE);
            table.bill.AddItem(item);

            return Ok(MapTableToDto(table));
        }

        // POST: api/tables/3/close
        // Body: { "paymentMethod": "cash" } or "card"
        [HttpPost("{id:int}/close")]
        public IActionResult CloseTable(int id, [FromBody] CloseTableRequest request)
        {
            var table = BarBillHolderLibrary.Models.Bar.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (!table.open)
                return BadRequest($"Table {id} is already closed.");

            if (table.bill == null)
                return BadRequest($"Table {id} has no bill.");

            if (BarBillHolderLibrary.Models.Bar.register == null)
                BarBillHolderLibrary.Models.Bar.register = new Register();

            var total = table.bill.total;
            decimal cardFee = 1.1M;

            switch (request.PaymentMethod?.ToLowerInvariant())
            {
                case "cash":
                    BarBillHolderLibrary.Models.Bar.register.cash += total;
                    break;
                case "card":
                    BarBillHolderLibrary.Models.Bar.register.card += total * cardFee;
                    break;
                default:
                    return BadRequest("PaymentMethod must be 'cash' or 'card'.");
            }

            // Your existing logic: sets open = false and clears bill
            table.Remove();

            return NoContent();
        }

        private static TableDto MapTableToDto(Table table)
        {
            var bill = table.bill ?? new Bill();

            return new TableDto(
                Id: table.ID,
                Name: table.name,
                Open: table.open,
                Total: bill.total,
                Items: bill.items?.Select(i => new BillItemDto(
                    Name: i.name,
                    Category: i.category,
                    Price: i.price
                )) ?? Enumerable.Empty<BillItemDto>()
            );
        }
    }

    public record TableDto(
        int Id,
        string Name,
        bool Open,
        decimal Total,
        IEnumerable<BillItemDto> Items
    );

    public record BillItemDto(
        string Name,
        string Category,
        decimal Price
    );

    public record AddItemRequest(
        string Name,
        string Category,
        decimal Price
    );

    public record CloseTableRequest(
        string PaymentMethod
    );
}
