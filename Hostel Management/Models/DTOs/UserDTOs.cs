using Hostel_Management.Areas.Identity.Data;
using Hostel_Management.Models.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hostel_Management.Models.DTOs
{

    public class UserWalletDTO
    {
     
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int CurrencyId { get; set; }



       
    }

    public class UserTransactionDTO
    {
       
        [Required]
        public Guid WalletId { get; set; }

        
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string TransactionType { get; set; }

        [MaxLength(255)]
        public string? Note { get; set; }  // Made nullable to prevent migration issues

    }
}
