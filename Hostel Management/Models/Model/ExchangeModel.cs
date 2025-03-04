using Hostel_Management.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hostel_Management.Models.Model
{
    public class ExchangeModel
    {

    }

    public class Wallet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]

        public string OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }

        [Required]
        public string ConnectedUserId { get; set; }
        [ForeignKey("ConnectedUserId")]
        public virtual ApplicationUser ConnectedUser { get; set; }
    }





        public class Currency
        {
        
     
        [Key]
            public int Id { get; set; }

            [Required]
            [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency code must be 3 letters (ISO 4217).")]
            public string Code { get; set; }

            [Required]
            [StringLength(100)]
            public string Name { get; set; }

           
           
            [Required]
            public string UserId { get; set; }
            [ForeignKey("UserId")]
            public virtual ApplicationUser User { get; set; }
        
    }


    public class BankAccount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string AccountName { set; get; }
        [Required]
        public string AccountNumber { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        public string AccountDisplay => $"{AccountName} ({AccountNumber})";
    }

    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int WalletId { get; set; }  // Ensure this is NOT nullable

        public virtual Wallet Wallet { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public int FromAccountId { get; set; }
        [ForeignKey("FromAccountId")]
        public virtual BankAccount FromAccount { get; set; }

        [Required]
        public int ToAccountId { get; set; }
        [ForeignKey("ToAccountId")]
        public virtual BankAccount ToAccount { get; set; }

        [Required]
        public int CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

}
