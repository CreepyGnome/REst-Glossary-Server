using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Regs.Infrastructure.Settings.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Regs.Infrastructure {
    public static class ApiDocumentationExtensions
    {
        private static readonly string OpenApiConfigKey = "OpenApi";

        public static IServiceCollection AddServiceApiDocumentationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenApiSettings>(options => configuration.GetSection(OpenApiConfigKey)
                                                                        .Bind(options));
            return services;
        }

        public static IServiceCollection AddServiceApiDocumentation(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                var openApiSettings = services.BuildServiceProvider()
                                              .GetService<IOptionsSnapshot<OpenApiSettings>>()
                                              .Value;
                options.OperationFilter<SwaggerDefaultValues>();
                AddSecurityDefinitionAndRequirements(openApiSettings, options);
                IncludeXmlComments(options);
            });

            return services;
        }

        private static void AddSecurityDefinitionAndRequirements(OpenApiSettings openApiSettings, SwaggerGenOptions options)
        {
            if (openApiSettings.Security != null)
            {
                options.AddSecurityDefinition(openApiSettings.Security.Scheme,
                                              new OpenApiSecurityScheme
                                              {
                                                  Description = openApiSettings.Security.Description,
                                                  Name = openApiSettings.Security.Name,
                                                  In = ParameterLocation.Header,
                                                  Scheme = openApiSettings.Security.Scheme,
                                                  Type = openApiSettings.Security.SchemeType,
                                                  BearerFormat = openApiSettings.Security.BearerFormat
                                              });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = openApiSettings.Security.Scheme}
                        },
                        new List<string>()
                    }
                });
            }
        }

        private static void IncludeXmlComments(SwaggerGenOptions options)
        {
            try
            {
                var xmlCommentsFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                options.IncludeXmlComments(xmlCommentsFullPath);
            }
            catch
            {
                // Ignore as this is dependent on a Project settings. Just means XML Comments will not be included if this fails.
            }
        }

        public static IApplicationBuilder UseServiceApiDocumentation(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, OpenApiSettings openApiSettings)
        {
            app.UseSwagger(options =>
            {
                var prefixPath = openApiSettings.BaseRelativePath;
                options.RouteTemplate = $"{prefixPath}/{{documentName}}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                var prefixPath = openApiSettings.BaseRelativePath;
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/{prefixPath}/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                options.RoutePrefix = openApiSettings.BaseRelativePath;
            });

            return app;
        }
    }
}