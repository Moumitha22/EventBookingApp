using System.ComponentModel.DataAnnotations;
using EventBookingApi.Models.Enums;

namespace EventBookingApi.Models.DTOs
{
    public class UserRegisterRequestDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;



        [Required(ErrorMessage = "Role is required.")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Role must be either 'Lister' or 'Buyer'.")]
        public UserRole Role { get; set; }
    }
}
