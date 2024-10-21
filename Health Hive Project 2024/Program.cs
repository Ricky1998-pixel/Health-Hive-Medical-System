using Health_Hive_Project_2024;
using Health_Hive_Project_2024.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Health_Hive_Project_2024.Hubs;
using Health_Hive_Project_2024.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add service registration for IEmailService
builder.Services.AddSingleton<IEmailService, EmailService>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
builder.Services.AddDefaultIdentity<MedicalProfessionalRecords>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false; // Disable the requirement for unique email addresses


    // Configure password options
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6; // Adjust the length as needed
})

    .AddRoles<IdentityRole>() // Add this line to configure RoleManager for IdentityRole
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSignalR();  // Add SignalR service
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

//Create a scope to resolve services
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Initialize RoleManager
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();// look in to

    // Check if "Admin" role exists, if not, create it
    if (!roleManager.RoleExistsAsync("Admin").Result)
    {
        var role = new IdentityRole("Admin");
        roleManager.CreateAsync(role).Wait();
    }

    // Check if "Nurse" role exists, if not, create it
    if (!roleManager.RoleExistsAsync("Nurse").Result)
    {
        var role = new IdentityRole("Nurse");
        roleManager.CreateAsync(role).Wait();
    }

    // Check if "Surgeon" role exists, if not, create it
    if (!roleManager.RoleExistsAsync("Surgeon").Result)
    {
        var role = new IdentityRole("Surgeon");
        roleManager.CreateAsync(role).Wait();
    }

    // Check if "Pharmacist" role exists, if not, create it
    if (!roleManager.RoleExistsAsync("Pharmacist").Result)
    {
        var role = new IdentityRole("Pharmacist");
        roleManager.CreateAsync(role).Wait();
    }

    // Check if "Anaesthesiologist" role exists, if not, create it
    if (!roleManager.RoleExistsAsync("Anesthesiologist").Result)
    {
        var role = new IdentityRole("Anesthesiologist");
        roleManager.CreateAsync(role).Wait();
    }
}






app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();



// Map the SignalR hub
app.MapHub<ChatHub>("/chathub");

app.Run();
