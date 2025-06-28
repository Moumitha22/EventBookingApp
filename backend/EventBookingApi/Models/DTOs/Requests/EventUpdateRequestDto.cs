namespace EventBookingApi.Models.DTOs
{
    public class EventUpdateRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public int TotalSeats { get; set; }
        public decimal Price { get; set; }
        public LocationCreateRequestDto Location { get; set; } = new();
    }
}