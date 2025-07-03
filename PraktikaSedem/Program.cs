using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PraktikaSedem.Data;

namespace PraktikaSedem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Use AddIdentity to support roles and UI
           builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders() // <-- Add this line!
    .AddDefaultUI();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); // Required for Identity UI

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
            app.MapRazorPages();

            // --- Role and admin user seeding block ---
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                // Create roles if they don't exist
                string[] roles = { "Admin", "Doctor", "Patient" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                
                var DoctorEmail = "doctor@gmail.com";
                var DoctorUser = await userManager.FindByEmailAsync(DoctorEmail);
                if (DoctorUser == null)
                {
                    DoctorUser = new IdentityUser { UserName = DoctorEmail, Email = DoctorEmail, EmailConfirmed = true };
                    await userManager.CreateAsync(DoctorUser, "Doctor123!"); // Change this password after first login
                }
                // Ensure user is in Admin role
                if (!await userManager.IsInRoleAsync(DoctorUser, "Doctor"))
                {
                    await userManager.AddToRoleAsync(DoctorUser, "Doctor");
                }

                // Create an admin user if needed
                var adminEmail = "admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                    await userManager.CreateAsync(adminUser, "Admin123!"); // Change this password after first login
                }
                // Ensure user is in Admin role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
          

            await app.RunAsync();
        }
    }
}