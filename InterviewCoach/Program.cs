using Microsoft.EntityFrameworkCore;
using InterviewCoach.Models;

namespace InterviewCoach
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllersWithViews();
            
            // Add DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=InterviewCoach.db";
            builder.Services.AddDbContext<InterviewCoachContext>(options =>
                options.UseSqlite(connectionString));

            // Add Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            
            // IMPORTANT: UseSession() must be after UseRouting()
            app.UseSession();

            // Replace app.MapControllers() with default routing:
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
