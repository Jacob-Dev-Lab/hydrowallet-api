using Microsoft.Extensions.DependencyInjection;
using Wallet.Application.Users.Registration;

namespace Wallet.Application.Extentions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<RegisterUserHandler>();

            return services;
        }
    }
}
