using GymQuest.Models;
using GymQuest.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GymQuest.Services;
using GymQuest.Data;

namespace GymQuest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Azure SQL Database connection string in user secrets. Will use for testing cloud DB later
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }

            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<WorkoutService>();
            builder.Services.AddScoped<WorkoutRepository>();
            builder.Services.AddScoped<ExerciseTrackingService>();
            builder.Services.AddScoped<ExerciseTrackingRepository>();

            // Configure DbContext with connection string
            builder.Services.AddDbContext<GymQuestDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"))); // Using Local SQL database for now

            // Register Identity services
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<GymQuestDbContext>()
                .AddDefaultTokenProviders();

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
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
