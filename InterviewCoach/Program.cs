using Microsoft.EntityFrameworkCore;

namespace InterviewCoach
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // Add DbContext for SQLite
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=InterviewCoach.db";
            // note to self: ?? is "null coalescing operator" - if the left side is null, use the right side instead
            builder.Services.AddDbContext<InterviewCoach.Models.InterviewCoachContext>(options =>
                options.UseSqlite(connectionString));

            // ADD THIS SESSION CONFIGURATION
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ADD THIS BEFORE MapControllers
            app.UseSession();

            app.MapControllers();

            app.Run();
        }
    }
}
