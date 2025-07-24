using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure database based on environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerConnection")));
}
else
{
    // Use SQLite in production (Azure) - simpler for portfolio
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseSqlite("Data Source=ContosoUniversity.db"));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Create database and tables if they don't exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SchoolContext>();
    
    try
    {
        // Ensure database is created
        context.Database.EnsureCreated();
        
        // Initialize with sample data only if no students exist
        if (!context.Students.Any())
        {
            DbInitializer.Initialize(context);
        }
    }
    catch (Exception ex)
    {
        // Log error but don't crash the application
        Console.WriteLine($"Database initialization error: {ex.Message}");
    }
}

app.Run();
