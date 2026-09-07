using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Application.Common.Interfaces;
using Wallet.Infrastructure.Data;
using Wallet.Infrastructure.Repositories;
using Wallet.Infrastructure.Services;

namespace Wallet.Infrastructure.Extensions
{
    public static class InfrastructureCollectionService
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
