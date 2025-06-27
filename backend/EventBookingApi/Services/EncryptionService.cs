using BCrypt.Net;
using EventBookingApi.Interfaces;

namespace EventBookingApi.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
