namespace EventBookingApi.Models
{
    public class Location
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Locality { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
