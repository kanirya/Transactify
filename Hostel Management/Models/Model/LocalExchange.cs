using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hostel_Management.Areas.Identity.Data;

namespace Hostel_Management.Models.Model
{
    public class UserWallet
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class UserTransaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid WalletId { get; set; }

        [ForeignKey("WalletId")]
        public virtual UserWallet Wallet { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [MaxLength(255)]
        public string? Note { get; set; }  // Made nullable to prevent migration issues

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public enum TransactionType
    {
        Credit,
        Debit
    }
}
