using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Domain.ValueObjects;
using Wallet.Infrastructure.Data;

namespace Wallet.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public async Task<bool> ExistsByEmailAsync(
            Email email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(
                    u => u.Email == email, 
                    cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(
            Email email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Email == email, 
                    cancellationToken);
        }

        public async Task<User?> GetByIdAsync(
            Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    u => u.Id == id,
                    cancellationToken);
        }
    }
}
