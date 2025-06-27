namespace EventBookingApi.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByToken { get; set; }

        // Computed Properties
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive => RevokedAt == null && !IsExpired;

        public User User { get; set; } = null!;
    }
}
