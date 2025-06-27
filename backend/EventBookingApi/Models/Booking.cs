namespace EventBookingApi.Models
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid EventId { get; set; }

        public DateTime BookedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation
        public User User { get; set; } = null!;

        public Event Event { get; set; } = null!;
    }
}
