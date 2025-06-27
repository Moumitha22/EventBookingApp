namespace EventBookingApi.Models.DTOs
{
    public class EventCreateRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public int TotalSeats { get; set; }
        public Guid CategoryId { get; set; }
        public LocationCreateRequestDto Location { get; set; } = new();
    }

}