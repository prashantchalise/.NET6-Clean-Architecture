using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
using PROJBP.UI.Data;
using PROJBP.UI.Models;

namespace PROJBP.UI.Modules
{

    public static class OAuthTokenModule
    {
        public static IServiceCollection OAuthSetupWithRefreshToken(this IServiceCollection services, IConfiguration configuration)
        {

            // Add services to the container.
            
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                // Register the entity sets needed by OpenIddict.
                options.UseOpenIddict();
            });



            // Register the Identity services.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();


            services.AddOpenIddict()

                            .AddValidation(options =>
                            {
                                // Import the configuration from the local OpenIddict server instance.
                                options.UseLocalServer();

                                // Register the ASP.NET Core host.
                                options.UseAspNetCore();
                            })

                            // Register the OpenIddict core components.
                            .AddCore(options =>
                            {
                                // Configure OpenIddict to use the EF Core stores/models.
                                options.UseEntityFrameworkCore()
                                                .UseDbContext<ApplicationDbContext>();
                            })

                            // Register the OpenIddict server components.
                            .AddServer(options =>
                            {
                                options
                                    .AllowClientCredentialsFlow()
                                    .AllowAuthorizationCodeFlow()
                                    .RequireProofKeyForCodeExchange()
                                    .AllowRefreshTokenFlow();


                                // Register the signing and encryption credentials.
                                options.AddDevelopmentEncryptionCertificate()
                                       .AddDevelopmentSigningCertificate();

                                options
                                    .SetTokenEndpointUris("/connect/token")
                                    .SetAuthorizationEndpointUris("/connect/authorize")
                                    .SetUserinfoEndpointUris("/connect/userinfo");

                                //// Encryption and signing of tokens
                                //options
                                //                .AddEphemeralEncryptionKey()
                                //                .AddEphemeralSigningKey()
                                //                .DisableAccessTokenEncryption();

                                // Register scopes (permissions)
                                options.RegisterScopes("api");



                                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                                options
                                    .UseAspNetCore()
                                    .EnableTokenEndpointPassthrough()
                                    .EnableAuthorizationEndpointPassthrough()
                                    .EnableUserinfoEndpointPassthrough();
                            });


           
            //builder.Services.AddHostedService<TestData>();
            services.AddHostedService<Worker>();
 
            return services;
        }
    }
     

}