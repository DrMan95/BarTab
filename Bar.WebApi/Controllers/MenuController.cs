using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BarBillHolderLibrary.Database; // for FileProcessor

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        public record DeleteMenuItemRequest(int Index);
        private static string MenuFilePath => FileProcessor.menuCSV;

        private static List<MenuItemDto>? _cachedMenu;
        private static readonly object _lock = new();

        public record MenuItemDto(
            string Name,
            string Category,
            decimal Price,
            bool Active
        );

        public record CreateMenuItemRequest(
            string Name,
            string Category,
            decimal Price,
            bool Active
        );

        // GET: api/menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll()
        {
            var items = await GetMenuAsync();

            var result = items
                .Select((item, idx) => new { item, idx })
                .Where(x => x.item.Active)
                .Select(x => new
                {
                    name = x.item.Name,
                    category = x.item.Category,
                    price = x.item.Price,
                    active = x.item.Active,
                    index = x.idx   // 👈 master list index
                });

            return Ok(result);
        }

        // GET: api/menu/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            var items = await GetMenuAsync();
            var categories = items
                .Where(i => i.Active)
                .Select(i => i.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            return Ok(categories);
        }

        // POST: api/menu
        // Body: { "name": "...", "category": "...", "price": 3.50, "active": true }
        [HttpPost]
        public async Task<ActionResult<MenuItemDto>> AddMenuItem([FromBody] CreateMenuItemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Category))
            {
                return BadRequest("Name and Category are required.");
            }

            if (request.Price <= 0)
            {
                return BadRequest("Price must be > 0.");
            }

            var items = await GetMenuAsync();

            var newItem = new MenuItemDto(
                Name: request.Name.Trim(),
                Category: request.Category.Trim(),
                Price: request.Price,
                Active: request.Active
            );

            lock (_lock)
            {
                items.Add(newItem);
                _cachedMenu = items;
            }

            await SaveMenuToCsvAsync();

            return CreatedAtAction(nameof(GetAll), newItem);
        }

        // DELETE: api/menu
        // Body: { "index": 3 }
        [HttpDelete]
        public async Task<IActionResult> DeleteMenuItem([FromBody] DeleteMenuItemRequest request)
        {
            var items = await GetMenuAsync();

            if (request.Index < 0 || request.Index >= items.Count)
                return BadRequest("Invalid index.");

            lock (_lock)
            {
                items.RemoveAt(request.Index);
                _cachedMenu = items;
            }

            await SaveMenuToCsvAsync();

            return NoContent();
        }


        // Optional: POST api/menu/reload (if you edit CSV manually while app is running)
        [HttpPost("reload")]
        public async Task<IActionResult> Reload()
        {
            await LoadMenuFromCsv(forceReload: true);
            return NoContent();
        }

        // ------------- internal helpers -------------

        private static async Task<List<MenuItemDto>> GetMenuAsync()
        {
            if (_cachedMenu != null) return _cachedMenu;
            await LoadMenuFromCsv(forceReload: true);
            return _cachedMenu ?? new List<MenuItemDto>();
        }

        private static async Task LoadMenuFromCsv(bool forceReload)
        {
            lock (_lock)
            {
                if (!forceReload && _cachedMenu != null)
                    return;
            }

            if (!System.IO.File.Exists(MenuFilePath))
            {
                lock (_lock)
                {
                    _cachedMenu = new List<MenuItemDto>();
                }
                return;
            }

            var lines = await System.IO.File.ReadAllLinesAsync(MenuFilePath);
            var list = new List<MenuItemDto>();

            bool first = true;

            foreach (var raw in lines)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    continue;

                // Detect & skip header line if present
                if (first &&
                    raw.Contains("Name", StringComparison.OrdinalIgnoreCase) &&
                    raw.Contains("Category", StringComparison.OrdinalIgnoreCase))
                {
                    first = false;
                    continue;
                }
                first = false;

                // Allow both ';' and ',' separators
                char sep = raw.Contains(';') ? ';' : ',';
                var parts = raw.Split(sep);
                if (parts.Length < 3) continue;

                var name = parts[0].Trim();
                var category = parts[1].Trim();

                if (!decimal.TryParse(
                        parts[2].Trim(),
                        NumberStyles.Number,
                        CultureInfo.InvariantCulture,
                        out var price))
                    continue;

                bool active = true;
                if (parts.Length >= 4)
                {
                    var activeText = parts[3].Trim();
                    if (!string.IsNullOrEmpty(activeText))
                    {
                        active =
                            activeText.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                            activeText == "1" ||
                            activeText.Equals("yes", StringComparison.OrdinalIgnoreCase);
                    }
                }

                list.Add(new MenuItemDto(name, category, price, active));
            }

            lock (_lock)
            {
                _cachedMenu = list;
            }
        }

        private static async Task SaveMenuToCsvAsync()
        {
            List<MenuItemDto> snapshot;
            lock (_lock)
            {
                snapshot = _cachedMenu?.ToList() ?? new List<MenuItemDto>();
            }

            var lines = new List<string>
            {
                "Name;Category;Price;Active"
            };

            foreach (var item in snapshot)
            {
                lines.Add(
                    $"{item.Name};{item.Category};" +
                    $"{item.Price.ToString(CultureInfo.InvariantCulture)};" +
                    $"{(item.Active ? "true" : "false")}"
                );
            }

            var dir = Path.GetDirectoryName(MenuFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            await System.IO.File.WriteAllLinesAsync(MenuFilePath, lines);
        }
    }
}
