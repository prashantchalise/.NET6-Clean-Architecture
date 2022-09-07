using PROJBP.UI.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddRazorPages();

// Add EF Module to DI
builder.Services.IncludeEFModule(builder.Configuration);

//Register Service Modules to DI
builder.Services.IncludeServiceModule();


//Add autentication Providers;
builder.Services.AddAutenticationProviders();

//Perform Oauth Setup using openiddict;
// Configure the Application db context, user manager and signin manager to use a single instance per request
builder.Services.OAuthSetupWithRefreshToken(builder.Configuration);


builder.Services.AddDatabaseDeveloperPageExceptionFilter();


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
