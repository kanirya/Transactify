using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hostel_Management.Models
{
    public enum Gender { Male, Female, Other }
    public enum BookingStatus { Active, Completed, Cancelled }
    public enum PaymentMethod { Cash, BankTransfer, Online }
    public enum PaymentStatus { Pending, Paid, Failed }
    public enum MaintenanceStatus { Pending, InProgress, Resolved }

    public class Floor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

    public class Room
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Floor")]
        public int FloorId { get; set; }

        [Required]
        [MaxLength(10)]
        public string RoomNumber { get; set; }

        [Range(1, 10)]
        public int Capacity { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Floor Floor { get; set; }

    }

    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
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

        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required]
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Active;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Student Student { get; set; }
        public Room Room { get; set; }
    }

    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        public Student Student { get; set; }
    }

    public class MaintenanceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public int? RequestedBy { get; set; }

        [Required]
        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Room Room { get; set; }
        public Student? RequestedStudent { get; set; }
    }
}