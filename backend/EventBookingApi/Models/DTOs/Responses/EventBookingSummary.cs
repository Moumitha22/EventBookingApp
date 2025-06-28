namespace EventBookingApi.Models.DTOs
{
    public class EventBookingSummaryDto
    {
        public Guid EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int TotalBookings { get; set; }
        public int TotalSeatsBooked { get; set; }

        public string Status { get; set; } = "Active";
    }


    
}