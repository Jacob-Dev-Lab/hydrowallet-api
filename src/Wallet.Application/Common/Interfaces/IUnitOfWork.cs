namespace Wallet.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsyn(CancellationToken cancellationToken = default);
        Task ExecuteTransactionAsync(
            Func<Task> action, 
            CancellationToken cancellationToken = default);
    }
}
