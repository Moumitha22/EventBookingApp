namespace EventBookingApi.Models.DTOs
{
    public class BookingResponseDto
    {
        public Guid BookingId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int SeatCount { get; set; }
        public decimal Price { get; set; }
        public DateTime BookedAt { get; set; }
    }

}