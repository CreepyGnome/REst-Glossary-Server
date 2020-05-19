using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Regs.Infrastructure.Settings.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>
    /// This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.
    /// </remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly OpenApiSettings _openApiSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        /// <param name="openApiSettings">The <see cref="OpenApiSettings">openApiSettings</see> used to get details to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IOptions<OpenApiSettings> openApiSettings)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _openApiSettings = openApiSettings?.Value ?? throw new ArgumentNullException(nameof(openApiSettings));
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = _openApiSettings.Title,
                Version = description.ApiVersion.ToString(),
                Description = _openApiSettings.Description,
                Contact = new OpenApiContact() { Name = _openApiSettings.ContactName, Email = _openApiSettings.ContactEmail },
                TermsOfService =  _openApiSettings.TermsOfServiceUrl,
                License = new OpenApiLicense() { Name = _openApiSettings.LicenseName, Url = _openApiSettings.LicenseUrl }
            };

            if (description.IsDeprecated)
                info.Description += _openApiSettings.DeprecatedDescriptionSuffix;

            return info;
        }
    }
}
