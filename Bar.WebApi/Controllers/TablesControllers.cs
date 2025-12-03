using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using BarBillHolderLibrary;          // Item, Customer
using BarBillHolderLibrary.Models;   // Bar, Table, Bill, Register
using BarBillHolderLibrary.Database; // FileProcessor

using BarState = BarBillHolderLibrary.Models.Bar;

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
            if (BarState.tables == null)
                return Ok(Enumerable.Empty<TableDto>());

            var result = BarState.tables.Select(MapTableToDto).ToList();
            return Ok(result);
        }

        // GET: api/tables/3
        [HttpGet("{id:int}")]
        public ActionResult<TableDto> GetTable(int id)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            return Ok(MapTableToDto(table));
        }

        // POST: api/tables
        // Creates a new table (extra / bar table)
        [HttpPost]
        public async Task<ActionResult<TableSummaryDto>> CreateTable()
        {
            if (BarState.tables == null)
                BarState.tables = new List<Table>();

            // Next ID = max existing ID + 1, or 1 if none
            int newId = BarState.tables.Any()
                ? BarState.tables.Max(t => t.ID) + 1
                : 1;

            var newTable = new Table(newId);  // open = false, empty bill
            BarState.tables.Add(newTable);

            await FileProcessor.SaveBarInstanceAsync();

            var dto = new TableSummaryDto(
                Id: newTable.ID,
                Name: newTable.name,
                Total: newTable.bill?.total ?? 0m,
                Open: newTable.open
            );

            // Action name must match GetTable above
            return CreatedAtAction(nameof(GetTable), new { id = dto.Id }, dto);
        }

        // POST: api/tables/3/items
        // Body: { "name": "...", "category": "...", "price": 2.50 }
        [HttpPost("{id:int}/items")]
        public ActionResult<TableDto> AddItemToTable(int id, [FromBody] AddItemRequest request)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (table.bill == null)
                table.bill = new Bill();

            table.open = true;

            var item = new Item(request.Name, request.Category, request.Price, Item.Status.UNDONE);
            table.bill.AddItem(item);

            // If you want to persist on every item add, you could make this async and call:
            // await FileProcessor.SaveBarInstanceAsync();

            return Ok(MapTableToDto(table));
        }

        // POST: api/tables/3/close
        // Body: { "paymentMethod": "cash", "tip": 1.00 } or "card"
        [HttpPost("{id:int}/close")]
        public async Task<IActionResult> CloseTable(int id, [FromBody] CloseTableRequest request)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (!table.open)
                return BadRequest($"Table {id} is already closed.");

            if (table.bill == null)
                return BadRequest($"Table {id} has no bill.");

            if (BarState.register == null)
                BarState.register = new Register();

            var total = table.bill.total;
            var tip = request.Tip ?? 0m;
            var method = request.PaymentMethod?.ToLowerInvariant();

            decimal multiplier;
            switch (method)
            {
                case "cash":
                    multiplier = 1.0m;
                    BarState.register.cash += total * multiplier + tip;
                    break;

                case "card":
                    // 10% extra fee
                    multiplier = 1.1m;
                    BarState.register.card += total * multiplier + tip;
                    break;

                default:
                    return BadRequest("PaymentMethod must be 'cash' or 'card'.");
            }

            // --- NEW: delete bar tables (ID > 14), keep base tables (1–14) as free ---
            const int BASE_TABLE_COUNT = 14;   // adjust if your fixed tables are a different count
            bool isBarTable = table.ID > BASE_TABLE_COUNT;

            if (isBarTable)
            {
                // Remove from the list entirely
                BarState.tables.Remove(table);
            }
            else
            {
                // Original behaviour: clear bill & mark as closed/free
                table.Remove();
            }
            // ------------------------------------------------------------------------//

            await FileProcessor.SaveBarInstanceAsync();

            return NoContent();
        }


        // POST: api/tables/3/move-to-table
        // Body: { "targetTableId": 5 }
        [HttpPost("{id:int}/move-to-table")]
        public async Task<IActionResult> MoveToTable(int id, [FromBody] MoveToTableRequest request)
        {
            if (request.TargetTableId <= 0)
                return BadRequest("TargetTableId must be > 0.");

            if (request.TargetTableId == id)
                return BadRequest("Cannot move to the same table.");

            var source = BarState.tables?.FirstOrDefault(t => t.ID == id);
            var target = BarState.tables?.FirstOrDefault(t => t.ID == request.TargetTableId);

            if (source == null)
                return NotFound($"Source table {id} not found.");
            if (target == null)
                return NotFound($"Target table {request.TargetTableId} not found.");

            if (source.bill == null || source.bill.items == null || source.bill.items.Count == 0)
                return BadRequest("Source table has no items to move.");

            if (target.bill == null)
                target.bill = new Bill();
            target.open = true;

            // Move all items from source to target
            var itemsToMove = source.bill.items.ToList();
            foreach (var item in itemsToMove)
            {
                target.bill.AddItem(item);
            }

            // Clear source table
            source.Remove();

            await FileProcessor.SaveBarInstanceAsync();

            return NoContent();
        }

        public record MoveToTableRequest(int TargetTableId);

        // OPTIONAL: still here if you ever want to move a table's bill to a named customer
        // POST: api/tables/3/move-to-customer
        // Body: { "customerName": "George" }
        [HttpPost("{id:int}/move-to-customer")]
        public async Task<IActionResult> MoveToCustomer(int id, [FromBody] MoveToCustomerRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerName))
                return BadRequest("CustomerName is required.");

            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (table.bill == null || table.bill.items == null || table.bill.items.Count == 0)
                return BadRequest("Table has no items to move.");

            if (BarState.customers == null)
                BarState.customers = new List<Customer>();

            var customer = BarState.customers.FirstOrDefault(c =>
                string.Equals(c.name, request.CustomerName, System.StringComparison.OrdinalIgnoreCase));

            if (customer == null)
                return NotFound($"Customer '{request.CustomerName}' not found.");

            if (customer.bill == null)
                customer.bill = new Bill();

            // Move all items
            var itemsToMove = table.bill.items.ToList();
            foreach (var item in itemsToMove)
            {
                customer.bill.AddItem(item);
            }

            table.Remove(); // table is now free

            await FileProcessor.SaveBarInstanceAsync();

            return NoContent();
        }

        public record MoveToCustomerRequest(string CustomerName);

        // ---------- mapping ----------

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

    // ---------- DTOs ----------

    // Full view with items
    public record TableDto(
        int Id,
        string Name,
        bool Open,
        decimal Total,
        IEnumerable<BillItemDto> Items
    );

    // Lightweight view (used by CreateTable)
    public record TableSummaryDto(
        int Id,
        string Name,
        decimal Total,
        bool Open
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
        string PaymentMethod,
        decimal? Tip
    );
}
