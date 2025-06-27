using EventBookingApi.Contexts;
using EventBookingApi.Interfaces;

namespace EventBookingApi.Services
{
    public class TransactionalService : ITransactionalService
    {
        private readonly EventBookingDbContext _context;

        public TransactionalService(EventBookingDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                await action();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await action();
                await tx.CommitAsync();
                return result;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }   

}