namespace EventBookingApi.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime DateTime { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }

        public Guid CategoryId { get; set; }

        public Guid LocationId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation
        public Category Category { get; set; } = null!;

        public Location Location { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
