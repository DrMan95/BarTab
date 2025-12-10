using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public record RemoveItemsRequest(List<int> ItemIndexes);

        private const int BaseTableCount = 14; // fixed tables (1..14), bar tables are > 14

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


        public record RenameTableRequest(string Name);

        [HttpPost("{id}/name")]
        public async Task<IActionResult> SetName(int id, [FromBody] RenameTableRequest request)
        {
            if (id <= 0 || id > BarState.tables.Count)
                return NotFound();

            var table = BarState.tables[id - 1];

            // Optional: trim and allow empty name
            var newName = request?.Name?.Trim() ?? string.Empty;
            table.name = newName; // or table.Name depending on your model

            await FileProcessor.SaveBarInstanceAsync();

            return NoContent();
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

            // --- Naming logic ---
            if (newId > BaseTableCount)
            {
                // This is a bar table → give it a Bar N name
                // Collect existing bar names so we don't duplicate
                var existingBarNames = BarState.tables
                    .Where(t => t.ID > BaseTableCount && !string.IsNullOrWhiteSpace(t.name))
                    .Select(t => t.name)
                    .ToHashSet(System.StringComparer.OrdinalIgnoreCase);

                int index = 1;
                string candidate;
                do
                {
                    candidate = $"Bar {index}";
                    index++;
                } while (existingBarNames.Contains(candidate));

                newTable.name = candidate;
            }
            else
            {
                // Normal "fixed" tables can keep whatever naming you already have,
                // but if you want to enforce a default, you can uncomment this:
                // newTable.name = newTable.name ?? $"Table {newId}";
            }

            BarState.tables.Add(newTable);

            await FileProcessor.SaveBarInstanceAsync();

            var dto = new TableSummaryDto(
                Id: newTable.ID,
                Name: newTable.name,
                Total: newTable.bill?.total ?? 0m,
                Open: newTable.open
            );

            return CreatedAtAction(nameof(GetTable), new { id = dto.Id }, dto);
        }


        // POST: api/tables/3/items
        // Body: { "name": "...", "category": "...", "price": 2.50 }
        [HttpPost("{id:int}/items")]
        public async Task<ActionResult<TableDto>> AddItemToTable(int id, [FromBody] AddItemRequest request)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (table.bill == null)
                table.bill = new Bill();

            table.open = true;

            var item = new Item(request.Name, request.Category, request.Price, Item.Status.UNDONE);
            table.bill.AddItem(item);

            await FileProcessor.SaveBarInstanceAsync();

            return Ok(MapTableToDto(table));
        }

        // POST: api/tables/3/close
        // Body: { "paymentMethod": "cash" } or "card"
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

            var tip = request.Tip ?? 0m;
            var discount = request.DiscountPercent ?? 0m;
            var total = table.bill.total - (discount/100 * table.bill.total);
            var method = request.PaymentMethod?.ToLowerInvariant();
            // 10% extra fee
            decimal multiplier = 1.1m;

            switch (method)
            {
                case "cash":
                    BarState.register.cash += total;
                    break;

                case "card":
                    BarState.register.card += total * multiplier;
                    break;

                default:
                    return BadRequest("PaymentMethod must be 'cash' or 'card'.");
            }
            BarState.register.tips += tip;

            FileProcessor.SaveToPaymentHistory(table.name, table.bill, tip);

            // --- HERE is the bar-table vs normal-table behaviour ---
            const int BaseTableCount = 14;           // same logic as JS BASE_TABLE_COUNT
            bool isBarTable = table.ID > BaseTableCount;

            if (isBarTable)
            {
                // Completely delete this extra/bar table from the list
                BarState.tables.Remove(table);
            }
            else
            {
                // Normal table: just clear bill and mark it free
                table.Remove();  // your existing method that clears bill + open=false
            }

            await FileProcessor.SaveBarInstanceAsync();

            return NoContent();
        }


        // POST: api/tables/3/remove-items
        // Body: { "itemIndexes": [0, 2, 3] }
        [HttpPost("{id:int}/remove-items")]
        public async Task<IActionResult> RemoveSelectedItems(int id, [FromBody] RemoveItemsRequest request)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (table.bill == null || table.bill.items == null || table.bill.items.Count == 0)
                return BadRequest("Table has no items.");

            if (request.ItemIndexes == null || request.ItemIndexes.Count == 0)
                return BadRequest("No items selected.");

            var billItems = table.bill.items;

            // Distinct, sorted indices
            var indices = request.ItemIndexes
                .Distinct()
                .OrderBy(i => i)
                .ToList();

            // Validate indices
            if (indices.Any(i => i < 0 || i >= billItems.Count))
                return BadRequest("One or more item indexes are invalid.");

            // Remove from highest index downwards
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                int idx = indices[i];
                var item = billItems[idx];
                table.bill.RemoveItem(item);
            }

            // If no items left, mark table as closed (but keep the table itself)
            if (table.bill.items.Count == 0)
            {
                table.open = false;
            }

            await FileProcessor.SaveBarInstanceAsync();

            return Ok(MapTableToDto(table));
        }


        // NEW: POST: api/tables/3/pay-items
        // Body: { "paymentMethod": "cash", "tip": 0.5, "itemIndexes": [0,2,3] }
        [HttpPost("{id:int}/pay-items")]
        public async Task<IActionResult> PaySelectedItems(int id, [FromBody] PayItemsRequest request)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (table.bill == null || table.bill.items == null || table.bill.items.Count == 0)
                return BadRequest("Table has no items to pay.");

            if (request.ItemIndexes == null || request.ItemIndexes.Count == 0)
                return BadRequest("No items selected.");

            if (BarState.register == null)
                BarState.register = new Register();

            var billItems = table.bill.items;

            // Distinct, sorted indices
            var indices = request.ItemIndexes
                .Distinct()
                .OrderBy(i => i)
                .ToList();

            // Validate indices
            if (indices.Any(i => i < 0 || i >= billItems.Count))
                return BadRequest("One or more item indexes are invalid.");

            // Calculate total of selected items
            decimal subtotal = 0m;
            Bill subBill = new Bill();
            foreach (var idx in indices)
            {
                subBill.AddItem(billItems[idx]);
                //subtotal += billItems[idx].price;
            }
            subtotal = subBill.total;

            var tip = request.Tip ?? 0m;
            var method = request.PaymentMethod?.ToLowerInvariant();

            decimal multiplier = 1.1m; // extra fee on selected items
            switch (method)
            {
                case "cash":
                    BarState.register.cash += subtotal;
                    break;

                case "card":
                    BarState.register.card += subtotal * multiplier;
                    break;

                default:
                    return BadRequest("PaymentMethod must be 'cash' or 'card'.");
            }
            BarState.register.tips += tip;

            FileProcessor.SaveToPaymentHistory(table.name, subBill, tip);

            // Remove selected items from the bill (from highest index downwards)
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                int idx = indices[i];
                var item = billItems[idx];
                table.bill.RemoveItem(item);
            }

            // If no items left, mark table as closed
            if (table.bill.items.Count == 0)
            {
                table.open = false;
            }

            await FileProcessor.SaveBarInstanceAsync();

            // Return updated table
            return Ok(MapTableToDto(table));
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

        // ---------- helpers & DTOs ----------

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

    // DTOs

    public record TableDto(
        int Id,
        string Name,
        bool Open,
        decimal Total,
        IEnumerable<BillItemDto> Items
    );

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
        decimal? Tip,
        decimal? DiscountPercent
    );

    public record MoveToTableRequest(
        int TargetTableId
    );

    public record PayItemsRequest(
        string PaymentMethod,
        decimal? Tip,
        List<int> ItemIndexes,
        decimal? DiscountPercent
    );
}
