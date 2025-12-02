using Microsoft.AspNetCore.Mvc;
using BarBillHolderLibrary.Models; // for Bar

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        // GET: api/menu
        [HttpGet]
        public ActionResult<IEnumerable<MenuItemDto>> GetMenu()
        {
            // If menu is null or empty, return empty list
            if (BarBillHolderLibrary.Models.Bar.menu == null || BarBillHolderLibrary.Models.Bar.menu.Count == 0)
            {
                return Ok(Enumerable.Empty<MenuItemDto>());
            }

            var result = new List<MenuItemDto>();

            // Bar.menu is: List<Tuple<string, List<Tuple<string, decimal>>>>
            foreach (var categoryTuple in BarBillHolderLibrary.Models.Bar.menu)
            {
                string categoryName = categoryTuple.Item1;          // e.g. "Coffee"
                var itemsInCategory = categoryTuple.Item2;          // List<Tuple<string, decimal>>

                foreach (var itemTuple in itemsInCategory)
                {
                    string itemName = itemTuple.Item1;              // e.g. "Espresso"
                    decimal price = itemTuple.Item2;                // e.g. 2.50m

                    result.Add(new MenuItemDto(
                        Name: itemName,
                        Category: categoryName,
                        Price: price
                    ));
                }
            }

            return Ok(result);
        }
    }

    public record MenuItemDto(
        string Name,
        string Category,
        decimal Price
    );
}
