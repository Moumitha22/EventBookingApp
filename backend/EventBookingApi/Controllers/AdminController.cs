using EventBookingApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EventBookingApi.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard-summary")] 
        public async Task<IActionResult> GetDashboardSummary()
        {
            var summary = await _adminService.GetDashboardSummaryAsync();
            return Ok(new { success = true, data = summary });
        }
    }
}
