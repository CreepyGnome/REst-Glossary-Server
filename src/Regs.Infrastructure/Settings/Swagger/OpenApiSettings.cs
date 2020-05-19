using System;
using Microsoft.OpenApi.Models;

namespace Regs.Infrastructure.Settings.Swagger
{
    public class OpenApiSettings
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DeprecatedDescriptionSuffix { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public Uri TermsOfServiceUrl { get; set; }
        public string LicenseName { get; set; }
        public Uri LicenseUrl { get; set; }
        public string BaseRelativePath { get; set; }
        public SecuritySettings Security { get; set; }

        public class SecuritySettings
        {
            public string Scheme { get; set; }
            public SecuritySchemeType SchemeType { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string BearerFormat { get; set; }
        }
    }
}
