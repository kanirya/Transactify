using Hostel_Management.Models;
using Microsoft.EntityFrameworkCore;
using Hostel_Management.Data;
using Microsoft.AspNetCore.Identity;
using Hostel_Management.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("TransactionPortal")
    ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

// Configure authentication
builder.Services.AddAuthentication().AddCookie();
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(
    options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AuthDbContext>();

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options => options.Password.RequireUppercase = false);

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

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Wallets}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
