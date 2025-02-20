using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Stripe;

namespace Hostel_Management.Models
{
    public enum RoomType { Single, Double, Shared }
    public enum RoomStatus { Available, Occupied, Maintenance }
    public enum Gender { Male, Female, Other }
    public enum BookingStatus { Active, Completed, Cancelled }
    public enum PaymentMethod { Cash, BankTransfer, Online }
    public enum PaymentStatus { Pending, Paid, Failed }
    public enum MaintenanceStatus { Pending, InProgress, Resolved }

    public class HostelRooms
    {

    }


    public class Floor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }

    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int FloorId { get; set; }
        public string RoomNumber { get; set; }
        public RoomType RoomType { get; set; }
        public int Capacity { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Floor Floor { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }

    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public int? RoomId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Room? Room { get; set; }
    }

    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Student Student { get; set; }
        public Room Room { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public Student Student { get; set; }

     
    }

    public class MaintenanceRequest
    {
        [Key]
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Description { get; set; }
        public int? RequestedBy { get; set; }
        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Room Room { get; set; }
        public Student? RequestedStudent { get; set; }
    }

}
