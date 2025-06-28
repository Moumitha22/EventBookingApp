using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Models.DTOs
{
    public class EventWithImageUploadDto
    {
        [FromForm]
        public string EventJson { get; set; } = string.Empty;

        [FromForm]
        public IFormFile? Image { get; set; }
    }
}
