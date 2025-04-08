using Microsoft.AspNetCore.Cors.Infrastructure;
using static Employees.Web.Infrastructure.WebConstants;

namespace Employees.Web.Infrastructure.Extensions;

public static class CorsOptionsExtensionClass
{
    public static void Configure(this CorsOptions options)
    {
        options.AddPolicy(CorsPolicyName,
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .WithMethods(HttpGet, HttpPost, HttpPut, HttpPatch, HttpDelete);
            });
    }
}

