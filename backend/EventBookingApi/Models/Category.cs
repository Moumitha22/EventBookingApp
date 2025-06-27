namespace EventBookingApi.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        // Navigation

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
