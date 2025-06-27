using System.ComponentModel.DataAnnotations;

namespace EventBookingApi.Models.DTOs
{
    public class LocationCreateRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Locality { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;
    }
}
