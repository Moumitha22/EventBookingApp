using Microsoft.EntityFrameworkCore;
using EventBookingApi.Contexts;
using EventBookingApi.Models;
using EventBookingApi.Exceptions;
using EventBookingApi.Interfaces;

namespace EventBookingApi.Repositories
{
    public class RefreshTokenRepository : Repository<Guid, RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(EventBookingDbContext context) : base(context) { }

        public override async Task<RefreshToken> Get(Guid key)
        {
            var token = await _eventBookingDbContext.RefreshTokens.SingleOrDefaultAsync(t => t.Id == key);
            return token ?? throw new NotFoundException($"Refresh token with ID {key} not found");
        }

        public override async Task<IEnumerable<RefreshToken>> GetAll()
        {
            return await _eventBookingDbContext.RefreshTokens.ToListAsync();
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _eventBookingDbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow);
        }

    }
}
