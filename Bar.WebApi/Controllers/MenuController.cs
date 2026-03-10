using Bar.WebApi.Data;
using Bar.WebApi.Data.Entities;
using Bar.WebApi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly BarDbContext _context;

        private readonly IHubContext<BarHub> _hub;

        public MenuController(BarDbContext context, IHubContext<BarHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        // JS expects: item.index, item.name, item.category, item.price, item.active
        // So we expose camelCase via JSON settings OR via these property names.
        public class MenuItemDto
        {
            public int index { get; set; }
            public string name { get; set; } = "";
            public string category { get; set; } = "";
            public decimal price { get; set; }
            public bool active { get; set; }
            public int? stockQuantity { get; set; }
        }

        public class CreateMenuItemRequest
        {
            public string Name { get; set; } = "";
            public string Category { get; set; } = "";
            public decimal Price { get; set; }
            public bool Active { get; set; } = true;

            // Your JS sends stockQuantity, so accept it
            public int? StockQuantity { get; set; }
        }

        public class UpdateMenuItemRequest
        {
            public string? Name { get; set; }
            public string? Category { get; set; }
            public decimal? Price { get; set; }
            public bool? Active { get; set; }
            public int? StockQuantity { get; set; }
        }

        // GET /api/menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetAll()
        {
            var items = await _context.MenuItems
                .Where(m => m.Active)
                .OrderBy(m => m.Category)
                .ThenBy(m => m.Name)
                .Select(m => new MenuItemDto
                {
                    index = m.Id,
                    name = m.Name,
                    category = m.Category,
                    price = m.Price,
                    active = m.Active,
                    stockQuantity = m.StockQuantity
                })
                .ToListAsync();

            return Ok(items);
        }

        // GET /api/menu/all
        // Admin view: returns active + inactive
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetAllIncludingInactive()
        {
            var items = await _context.MenuItems
                .OrderBy(m => m.Category)
                .ThenBy(m => m.Name)
                .Select(m => new MenuItemDto
                {
                    index = m.Id,
                    name = m.Name,
                    category = m.Category,
                    price = m.Price,
                    active = m.Active,
                    stockQuantity = m.StockQuantity
                })
                .ToListAsync();

            return Ok(items);
        }


        // GET /api/menu/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            var categories = await _context.MenuItems
                .Where(m => m.Active)
                .Select(m => m.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(categories);
        }

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

            if (request.StockQuantity is not null && request.StockQuantity < 0)
            {
                return BadRequest("StockQuantity must be >= 0 or null.");
            }

            var entity = new MenuItem
            {
                Name = request.Name.Trim(),
                Category = request.Category.Trim(),
                Price = request.Price,
                Active = request.Active,
                StockQuantity = request.StockQuantity
            };

            _context.MenuItems.Add(entity);
            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("RefreshAll");

            var dto = new MenuItemDto
            {
                index = entity.Id,
                name = entity.Name,
                category = entity.Category,
                price = entity.Price,
                active = entity.Active,
                stockQuantity = entity.StockQuantity
            };

            return CreatedAtAction(nameof(GetAll), dto);
        }

        // DELETE /api/menu/{id}
        // JS currently calls: fetch("/api/menu/" + id, { method:"DELETE" })
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("RefreshAll");
            return NoContent();
        }

        // Optional: PUT /api/menu/{id}
        // This is useful if you later add "edit price/stock" in UI.
        [HttpPut("{id:int}")]
        public async Task<ActionResult<MenuItemDto>> Update(int id, [FromBody] UpdateMenuItemRequest request)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return NotFound();

            if (request.Name != null) item.Name = request.Name.Trim();
            if (request.Category != null) item.Category = request.Category.Trim();
            if (request.Price != null) item.Price = request.Price.Value;
            if (request.Active != null) item.Active = request.Active.Value;

            if (request.StockQuantity != null && request.StockQuantity < 0)
                return BadRequest("StockQuantity must be >= 0 or null.");

            if (request.StockQuantity != null)
                item.StockQuantity = request.StockQuantity;
            else
                item.StockQuantity = null;

            await _context.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("RefreshAll");
            return Ok(new MenuItemDto
            {
                index = item.Id,
                name = item.Name,
                category = item.Category,
                price = item.Price,
                active = item.Active,
                stockQuantity = item.StockQuantity
            });
        }
    }
}
