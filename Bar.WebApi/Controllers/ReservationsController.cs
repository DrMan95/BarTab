using Microsoft.AspNetCore.Mvc;
using Bar.WebApi.Models;

namespace Bar.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        // GET: api/reservations
        [HttpGet]
        public ActionResult<IEnumerable<ReservationDto>> GetAll()
        {
            var all = ReservationStore.GetAll()
                .OrderBy(r => r.Time)
                .ThenBy(r => r.Id)
                .Select(r => new ReservationDto(
                    Id: r.Id,
                    TableId: r.TableId,
                    Name: r.Name,
                    Time: r.Time,
                    Notes: r.Notes,
                    Status: r.Status.ToString()
                ));

            return Ok(all);
        }

        // POST: api/reservations
        // Body: { "tableId": 3, "name": "John", "time": "2025-12-02T20:00:00", "notes": "Birthday" }
        [HttpPost]
        public ActionResult<ReservationDto> Create([FromBody] CreateReservationRequest req)
        {
            if (req.TableId <= 0)
                return BadRequest("TableId must be > 0.");
            if (string.IsNullOrWhiteSpace(req.Name))
                return BadRequest("Name is required.");

            if (!DateTime.TryParse(req.Time, out var parsed))
                return BadRequest("Time must be a valid date/time string.");

            var res = ReservationStore.Add(req.TableId, req.Name, parsed, req.Notes);

            var dto = new ReservationDto(
                Id: res.Id,
                TableId: res.TableId,
                Name: res.Name,
                Time: res.Time,
                Notes: res.Notes,
                Status: res.Status.ToString()
            );

            return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
        }

        // PUT: api/reservations/5/status
        // Body: { "status": "Completed" } or "Cancelled"
        [HttpPut("{id:int}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateReservationStatusRequest req)
        {
            if (!Enum.TryParse<ReservationStatus>(req.Status, ignoreCase: true, out var status))
                return BadRequest("Status must be Upcoming, Completed or Cancelled.");

            var ok = ReservationStore.TryUpdateStatus(id, status);
            if (!ok) return NotFound($"Reservation {id} not found.");

            return NoContent();
        }
    }

    public record ReservationDto(
        int Id,
        int TableId,
        string Name,
        DateTime Time,
        string? Notes,
        string Status
    );

    public record CreateReservationRequest(
        int TableId,
        string Name,
        string Time,
        string? Notes
    );

    public record UpdateReservationStatusRequest(
        string Status
    );
}
