using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace PROJBP.UI.Modules
{

    public static class AddAuthProvidersExtension
    {
        public static void AddAutenticationProviders(this IServiceCollection builder)
        {
            builder
                 .AddAuthentication()
                .AddJwtBearer();
    

        builder.ConfigureApplicationCookie(config =>
        {
            config.LoginPath = "/Identity/Account/LogIn";
            config.AccessDeniedPath = "/Identity/Account/LogIn";
            config.SlidingExpiration = true;
            config.Events = new CustomCookieAuthenticationEvents();
            config.Cookie.SameSite = SameSiteMode.None;
        });
            builder.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme, JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

 
        }

    }



    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.StatusCode == StatusCodes.Status200OK)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.WriteAsJsonAsync("Authorization has been denied.");
                return Task.CompletedTask;
            }
            else
            {
                return base.RedirectToLogin(context);
            }
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            if (context.Request.Path.StartsWithSegments("/api") &&
                context.Response.StatusCode == StatusCodes.Status200OK)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.WriteAsJsonAsync("Authorization has been denied.");
                return Task.CompletedTask;
            }
            else
            {
                return base.RedirectToAccessDenied(context);
            }
        }
    }


}