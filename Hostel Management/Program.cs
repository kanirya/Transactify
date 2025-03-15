using Hostel_Management.Models;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;
using Microsoft.AspNetCore.Identity;
using Hostel_Management.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TransactionPortal")
    ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

// Configure DbContext
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

// Configure Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AuthDbContext>();

// Configure Application Cookie to ensure correct login redirection
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Ensure Authentication is before Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserWallets}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
