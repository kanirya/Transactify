using Hostel_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Hostel_Management.Data
{
    
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Floor> Floors { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }


}
}
