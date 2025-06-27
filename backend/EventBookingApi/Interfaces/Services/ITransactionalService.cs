namespace EventBookingApi.Interfaces
{
    public interface ITransactionalService
    {
        Task ExecuteInTransactionAsync(Func<Task> action);
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action);
    }

}