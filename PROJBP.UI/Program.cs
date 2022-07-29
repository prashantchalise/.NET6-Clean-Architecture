using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PROJBP.UI.Data;
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
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();


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

app.Run();
