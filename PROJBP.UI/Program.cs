using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PROJBP.UI.Data;
using PROJBP.UI.Models;
using PROJBP.UI.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

// Add EF Module to DI
builder.Services.IncludeEFModule(builder.Configuration);

//Register Service Modules to DI
builder.Services.IncludeServiceModule(builder.Configuration);


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(connectionString);

        // Register the entity sets needed by OpenIddict.
        options.UseOpenIddict();
    });



builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// Register the Identity services.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();




builder.Services.AddOpenIddict()

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

                    options
                        .SetTokenEndpointUris("/connect/token")
                        .SetAuthorizationEndpointUris("/connect/authorize")
                        .SetUserinfoEndpointUris("/connect/userinfo");

                    // Encryption and signing of tokens
                    options
                        .AddEphemeralEncryptionKey()
                        .AddEphemeralSigningKey()
                        .DisableAccessTokenEncryption();

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
builder.Services.AddHostedService<Worker>();


builder.Services.AddRazorPages();

//Add Controllers with views;
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
