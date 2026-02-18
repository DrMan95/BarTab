using Bar.WebApi.Data;
using BarBillHolderLibrary;          // Item, Customer
using BarBillHolderLibrary.Database; // FileProcessor
using BarBillHolderLibrary.Models;   // Bar, Table, Bill, Register
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BarState = BarBillHolderLibrary.Models.Bar;

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        private const int BaseTableCount = 14; // fixed tables (1..14), bar tables are > 14

        private readonly BarDbContext _db;

        public TablesController(BarDbContext db)
        {
            _db = db;
        }

        // -------------------- DTOs / Requests --------------------

        public record RemoveItemsRequest(List<int> ItemIndexes);

        public record RenameTableRequest(string Name);

        public record AddItemRequest
        {
            public int? MenuItemId { get; init; }
            public string? Name { get; init; }
            public string? Category { get; init; }
            public decimal? Price { get; init; }
        }

        public record CloseTableRequest(
            string PaymentMethod,
            decimal? Tip,
            decimal? DiscountPercent
        );

        public record MoveToTableRequest(int TargetTableId);

        public record PayItemsRequest(
            string PaymentMethod,
            decimal? Tip,
            List<int> ItemIndexes,
            decimal? DiscountPercent
        );

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

        // ✅ add MenuItemId so we can debug / extend later (frontend can ignore it)
        public record BillItemDto(
            int? MenuItemId,
            string Name,
            string Category,
            decimal Price
        );

        // -------------------- Endpoints --------------------

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

        // POST: api/tables/{id}/name
        [HttpPost("{id}/name")]
        public async Task<IActionResult> SetName(int id, [FromBody] RenameTableRequest request)
        {
            if (id <= 0 || BarState.tables == null || id > BarState.tables.Count)
                return NotFound();

            var table = BarState.tables[id - 1];

            var newName = request?.Name?.Trim() ?? string.Empty;
            table.name = newName;

            await FileProcessor.SaveBarInstanceAsync();
            return NoContent();
        }

        // POST: api/tables (creates a new bar table)
        [HttpPost]
        public async Task<ActionResult<TableSummaryDto>> CreateTable()
        {
            BarState.tables ??= new List<Table>();

            int newId = BarState.tables.Any()
                ? BarState.tables.Max(t => t.ID) + 1
                : 1;

            var newTable = new Table(newId);

            if (newId > BaseTableCount)
            {
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
        // Preferred body: { "menuItemId": 12 }
        // Legacy body: { "name": "...", "category": "...", "price": 2.50 }
        [HttpPost("{id:int}/items")]
        public async Task<ActionResult<TableDto>> AddItemToTable(int id, [FromBody] AddItemRequest request)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            table.bill ??= new Bill();
            table.open = true;

            // Preferred: add by MenuItemId (DB-backed, enables stock decrement)
            if (request.MenuItemId.HasValue && request.MenuItemId.Value > 0)
            {
                var menuItem = await _db.MenuItems
                    .FirstOrDefaultAsync(m => m.Id == request.MenuItemId.Value && m.Active);

                if (menuItem == null)
                    return NotFound("Menu item not found or inactive.");

                // Stock enforcement (null = unlimited)
                if (menuItem.StockQuantity.HasValue)
                {
                    if (menuItem.StockQuantity.Value <= 0)
                        return BadRequest("Out of stock.");

                    menuItem.StockQuantity = menuItem.StockQuantity.Value - 1;
                    await _db.SaveChangesAsync();
                }

                var item = new Item(menuItem.Name, menuItem.Category, menuItem.Price, Item.Status.UNDONE)
                {
                    // ✅ CRITICAL: remember where this came from so we can restock on cancel
                    MenuItemId = menuItem.Id
                };

                table.bill.AddItem(item);

                await FileProcessor.SaveBarInstanceAsync();
                return Ok(MapTableToDto(table));
            }

            // Legacy fallback
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Category) ||
                request.Price is null ||
                request.Price.Value <= 0)
            {
                return BadRequest("Either provide menuItemId, or provide name/category/price.");
            }

            var fallbackItem = new Item(
                request.Name.Trim(),
                request.Category.Trim(),
                request.Price.Value,
                Item.Status.UNDONE);

            table.bill.AddItem(fallbackItem);

            await FileProcessor.SaveBarInstanceAsync();
            return Ok(MapTableToDto(table));
        }

        // POST: api/tables/3/close
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

            BarState.register ??= new Register();

            var tip = request.Tip ?? 0m;
            var discount = request.DiscountPercent ?? 0m;

            var total = table.bill.total - (discount / 100m * table.bill.total);

            var method = request.PaymentMethod?.ToLowerInvariant();
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

            bool isBarTable = table.ID > BaseTableCount;

            if (isBarTable)
                BarState.tables.Remove(table);
            else
                table.Remove();

            await FileProcessor.SaveBarInstanceAsync();
            return NoContent();
        }

        // ✅ CANCEL / REMOVE selected items AND RESTOCK
        // POST: api/tables/3/remove-items
        [HttpPost("{id:int}/remove-items")]
        public async Task<IActionResult> RemoveSelectedItems(int id, [FromBody] RemoveItemsRequest request)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (table.bill?.items == null || table.bill.items.Count == 0)
                return BadRequest("Table has no items.");

            if (request.ItemIndexes == null || request.ItemIndexes.Count == 0)
                return BadRequest("No items selected.");

            var billItems = table.bill.items;

            var indices = request.ItemIndexes
                .Distinct()
                .OrderBy(i => i)
                .ToList();

            if (indices.Any(i => i < 0 || i >= billItems.Count))
                return BadRequest("One or more item indexes are invalid.");

            // Remove from highest index downwards
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                int idx = indices[i];
                var item = billItems[idx];

                // ✅ RESTOCK in SQLite if this item came from menu
                await RestockIfNeeded(item);

                // Remove from bill
                table.bill.RemoveItem(item);
            }

            await _db.SaveChangesAsync();

            if (table.bill.items.Count == 0)
                table.open = false;

            await FileProcessor.SaveBarInstanceAsync();
            return Ok(MapTableToDto(table));
        }

        // POST: api/tables/3/pay-items
        // ✅ Do NOT restock here (these are paid)
        [HttpPost("{id:int}/pay-items")]
        public async Task<IActionResult> PaySelectedItems(int id, [FromBody] PayItemsRequest request)
        {
            var table = BarState.tables?.FirstOrDefault(t => t.ID == id);
            if (table == null)
                return NotFound($"Table {id} not found.");

            if (table.bill?.items == null || table.bill.items.Count == 0)
                return BadRequest("Table has no items to pay.");

            if (request.ItemIndexes == null || request.ItemIndexes.Count == 0)
                return BadRequest("No items selected.");

            BarState.register ??= new Register();

            var billItems = table.bill.items;

            var indices = request.ItemIndexes
                .Distinct()
                .OrderBy(i => i)
                .ToList();

            if (indices.Any(i => i < 0 || i >= billItems.Count))
                return BadRequest("One or more item indexes are invalid.");

            // Build a sub-bill for history + total calc
            Bill subBill = new Bill();
            foreach (var idx in indices)
                subBill.AddItem(billItems[idx]);

            var subtotal = subBill.total;

            var tip = request.Tip ?? 0m;
            var discount = request.DiscountPercent ?? 0m;

            // Apply discount to selected subtotal too (recommended, since your UI sends it)
            var subtotalAfterDiscount = subtotal - (discount / 100m * subtotal);

            var method = request.PaymentMethod?.ToLowerInvariant();
            decimal multiplier = 1.1m;

            switch (method)
            {
                case "cash":
                    BarState.register.cash += subtotalAfterDiscount;
                    break;
                case "card":
                    BarState.register.card += subtotalAfterDiscount * multiplier;
                    break;
                default:
                    return BadRequest("PaymentMethod must be 'cash' or 'card'.");
            }

            BarState.register.tips += tip;
            FileProcessor.SaveToPaymentHistory(table.name, subBill, tip);

            // Remove paid items from bill
            for (int i = indices.Count - 1; i >= 0; i--)
            {
                int idx = indices[i];
                var item = billItems[idx];
                table.bill.RemoveItem(item);
            }

            if (table.bill.items.Count == 0)
                table.open = false;

            await FileProcessor.SaveBarInstanceAsync();
            return Ok(MapTableToDto(table));
        }

        // POST: api/tables/3/move-to-table
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

            if (source.bill?.items == null || source.bill.items.Count == 0)
                return BadRequest("Source table has no items to move.");

            target.bill ??= new Bill();
            target.open = true;

            // Move all items
            var itemsToMove = source.bill.items.ToList();
            foreach (var item in itemsToMove)
                target.bill.AddItem(item); // MenuItemId stays on item (good)

            source.Remove();

            await FileProcessor.SaveBarInstanceAsync();
            return NoContent();
        }

        // -------------------- Helpers --------------------

        private async Task RestockIfNeeded(Item billItem)
        {
            // If you didn’t add MenuItemId to Item, this will always be null → no restock
            if (billItem.MenuItemId == null || billItem.MenuItemId.Value <= 0)
                return;

            var menuItem = await _db.MenuItems.FirstOrDefaultAsync(m => m.Id == billItem.MenuItemId.Value);
            if (menuItem == null)
                return;

            // Only restock limited items (null = unlimited)
            if (menuItem.StockQuantity.HasValue)
                menuItem.StockQuantity = menuItem.StockQuantity.Value + 1;
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
                    MenuItemId: i.MenuItemId,
                    Name: i.name,
                    Category: i.category,
                    Price: i.price
                )) ?? Enumerable.Empty<BillItemDto>()
            );
        }
    }
}
