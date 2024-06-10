using Fleet_Management.Models;
using Fleet_Management.Data;
using FluentAssertions.Common;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Principal;
using System.Text.Json.Serialization;
using System.Text.Json;
using Fleet_Management;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddCors(options =>
{
     options.AddPolicy("MyAllowSpecificOrigins",
            builder =>
            {
                builder.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });

});

// للـ MVC
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
           .EnableSensitiveDataLogging() 
           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information));


builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = null;
        options.JsonSerializerOptions.WriteIndented = true;  // If you prefer pretty JSON
        options.JsonSerializerOptions.PropertyNamingPolicy = null; 
    });

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); 


builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddSignalR();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("MyAllowSpecificOrigins");
 

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");


    endpoints.MapControllerRoute(
        name: "geofences",
        pattern: "Geofences",
        defaults: new { controller = "Geofences", action = "Index" });


    endpoints.MapControllerRoute(
        name: "vehicles",
        pattern: "Vehicles",
        defaults: new { controller = "Vehicles", action = "Index" });


    endpoints.MapControllerRoute(
        name: "routeHistory",
        pattern: "{controller=RouteHistory}/{action=RouteHistory}/{id?}");
    endpoints.MapHub<RouteHistoryHub>("/routeHistoryHub");

    endpoints.MapControllers(); 
});

app.Run();





