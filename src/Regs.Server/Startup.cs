using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Regs.Infrastructure;
using Regs.Infrastructure.Settings.Swagger;
using Regs.Server.Services;

namespace Regs.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.CheckAdditionalContent = true;
                        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .ConfigureApiBehaviorOptions(options =>
                    {
                        options.SuppressMapClientErrors = true;
                    });
            services.AddServiceApiDocumentationSettings(Configuration);
            services.AddServiceApiVersioning();
            services.AddServiceApiDocumentation();
            services.AddSingleton<RegsDatabaseManager>();
            services.AddScoped<IRegsService, RegsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
                              IApiVersionDescriptionProvider provider, 
                              IOptionsSnapshot<OpenApiSettings> openApiSettings)
        {
            app.UseGlobalExceptionHandler();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseServiceApiDocumentation(provider, openApiSettings.Value);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
