using EventBookingApi.Models.DTOs;

namespace EventBookingApi.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync();
    }

}