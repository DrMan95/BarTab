namespace Bar.WebApi.Models
{
    public enum ReservationStatus
    {
        Upcoming,
        Completed,
        Cancelled
    }

    public class Reservation
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public string? Notes { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Upcoming;
    }

    /// <summary>
    /// Simple in-memory store for reservations.
    /// (Survives while the app runs; resets when you restart the app.)
    /// </summary>
    public static class ReservationStore
    {
        private static readonly List<Reservation> _reservations = new();
        private static int _nextId = 1;
        private static readonly object _lock = new();

        public static IEnumerable<Reservation> GetAll()
        {
            lock (_lock)
            {
                // Clone so callers can't mutate internal list
                return _reservations
                    .Select(r => new Reservation
                    {
                        Id = r.Id,
                        TableId = r.TableId,
                        Name = r.Name,
                        Time = r.Time,
                        Notes = r.Notes,
                        Status = r.Status
                    })
                    .ToList();
            }
        }

        public static Reservation Add(int tableId, string name, DateTime time, string? notes)
        {
            lock (_lock)
            {
                var res = new Reservation
                {
                    Id = _nextId++,
                    TableId = tableId,
                    Name = name,
                    Time = time,
                    Notes = notes,
                    Status = ReservationStatus.Upcoming
                };

                _reservations.Add(res);
                return res;
            }
        }

        public static bool TryUpdateStatus(int id, ReservationStatus status)
        {
            lock (_lock)
            {
                var res = _reservations.FirstOrDefault(r => r.Id == id);
                if (res == null) return false;

                res.Status = status;
                return true;
            }
        }
    }
}
