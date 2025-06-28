namespace EventBookingApi.Models.DTOs
{
    public class AdminDashboardSummaryDto
    {
        public int TotalEvents { get; set; }
        public int TotalUsers { get; set; }
        public int TotalBookings { get; set; }
        public int UpcomingEventsCount { get; set; }
        public int SoldOutEventsCount { get; set; }
    }

}