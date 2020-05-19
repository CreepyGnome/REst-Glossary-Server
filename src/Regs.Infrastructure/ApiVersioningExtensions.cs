using Microsoft.Extensions.DependencyInjection;

namespace Regs.Infrastructure
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddServiceApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
