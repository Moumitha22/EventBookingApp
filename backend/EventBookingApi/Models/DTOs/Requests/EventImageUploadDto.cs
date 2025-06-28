using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi
{
    public class EventImageUploadDto
    {
        [FromForm]
        public Guid EventId { get; set; }

        [FromForm]
        public IFormFile File { get; set; } = null!;
    }

   
}