using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EventBookingApi.Models.DTOs
{
    [Keyless]
    public class BookingIdResult
    {

        [Column("bookingid")]
        public Guid BookingId { get; set; }
    }
}
