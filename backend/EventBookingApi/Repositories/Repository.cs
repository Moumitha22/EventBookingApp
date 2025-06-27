using EventBookingApi.Interfaces;
using EventBookingApi.Contexts;
using EventBookingApi.Exceptions;

namespace EventBookingApi.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly EventBookingDbContext _eventBookingDbContext;

        public Repository(EventBookingDbContext eventBookingDbContext)
        {
            _eventBookingDbContext = eventBookingDbContext;
        }

        public async Task<T> Add(T item)
        {
            if (item == null)
                throw new BadRequestException("Cannot add null entity");

            _eventBookingDbContext.Add(item);
            await _eventBookingDbContext.SaveChangesAsync();
            return item;
        }

        public async Task<T> Delete(K key)
        {
            var item = await Get(key);
            _eventBookingDbContext.Remove(item);
            await _eventBookingDbContext.SaveChangesAsync();
            return item;
        }

        public abstract Task<T> Get(K key);

        public abstract Task<IEnumerable<T>> GetAll();

        public async Task<T> Update(K key, T item)
        {
            var myItem = await Get(key);
            if (myItem != null)
            {
                _eventBookingDbContext.Entry(myItem).CurrentValues.SetValues(item);
                await _eventBookingDbContext.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }
    }
}
