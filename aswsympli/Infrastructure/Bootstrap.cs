using Application.Abstraction;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class Bootstrap
    {
        public static IApplicationBuilder BootstrapInfrastructure(this IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var appDb = scope.ServiceProvider.GetService<IApplicationDb>() as DbContext;
            appDb.Database.Migrate();

            return applicationBuilder;
        }
    }
}
