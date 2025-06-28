using EventBookingApi.Interfaces;
using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IBookingRepository _bookingRepository;

        public AdminService(IUserRepository userRepository, IEventRepository eventRepository, IBookingRepository bookingRepository)
        {
            _userRepository = userRepository;
            _eventRepository = eventRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var users = await _userRepository.GetAll();
            var events = await _eventRepository.GetAll(); 
            var bookingSummaries = await _bookingRepository.GetEventBookingSummariesAsync();

            return new AdminDashboardSummaryDto
            {
                TotalUsers = users.Count(),
                TotalEvents = events.Count(),
                TotalBookings = bookingSummaries.Sum(b => b.TotalBookings),
                UpcomingEventsCount = events.Count(e => e.DateTime > DateTime.UtcNow),
                SoldOutEventsCount = bookingSummaries.Count(b => b.AvailableSeats == 0)
            };
        }
    }   
}