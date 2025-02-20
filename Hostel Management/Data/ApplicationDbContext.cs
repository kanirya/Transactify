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


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
            .HasOne(r => r.Floor)
            .WithMany(f => f.Rooms)
            .HasForeignKey(r => r.FloorId);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.Room)
            .WithMany(r => r.Students)
            .HasForeignKey(s => s.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Student)
            .WithMany()
            .HasForeignKey(b => b.StudentId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany()
            .HasForeignKey(b => b.RoomId);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Student)
            .WithMany()
            .HasForeignKey(p => p.StudentId);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(m => m.Room)
            .WithMany()
            .HasForeignKey(m => m.RoomId);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(m => m.RequestedStudent)
            .WithMany()
            .HasForeignKey(m => m.RequestedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
}
