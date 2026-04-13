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


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
