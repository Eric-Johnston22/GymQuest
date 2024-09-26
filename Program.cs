using GymQuest.Models;
using GymQuest.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GymQuest.Services;
using GymQuest.Data;
using Azure.Identity;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace GymQuest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Read configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole(); // Add any other logging providers here, like Azure, Debug, etc.
            builder.Logging.AddDebug();
            builder.Logging.AddAzureWebAppDiagnostics(); // Optional: Log to Azure App Service diagnostics


            //Azure SQL Database connection string in user secrets. Will use for testing cloud DB later
            //if (builder.Environment.IsDevelopment())
            //    {
            //        builder.Configuration.AddUserSecrets<Program>();
            //    }

            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<WorkoutService>();
            builder.Services.AddScoped<WorkoutRepository>();
            builder.Services.AddScoped<ExerciseTrackingService>();
            builder.Services.AddScoped<ExerciseTrackingRepository>();

            // Azure Key Vault configuration
            var keyVaultName = builder.Configuration["KeyVault"];
            var keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";  // Key Vault URI as string
            Console.WriteLine($"Key Vault name: {keyVaultName}");

            // Use DefaultAzureCredential for modern Azure Identity authentication
            var credential = new DefaultAzureCredential();

            // Correct overload from Azure.Extensions.AspNetCore.Configuration.Secrets
            builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), credential);

            // Retrive database connection string
            var connectionString = builder.Configuration["AzureSQLDatabase"];

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("Connection string is null or empty.");
            }
            else
            {
                Console.WriteLine($"Connection string retrieved: {connectionString}");
            }

            // Configure DbContext with connection string
            builder.Services.AddDbContext<GymQuestDbContext>(options =>
                options.UseSqlServer(connectionString));

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
