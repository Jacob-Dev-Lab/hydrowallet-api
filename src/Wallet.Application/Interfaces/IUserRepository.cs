using Wallet.Domain.Entities;
using Wallet.Domain.ValueObjects;

namespace Wallet.Application.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
        Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken);
    }
}
