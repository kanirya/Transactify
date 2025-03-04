using Hostel_Management.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hostel_Management.Models.DTOs
{

    public class GeneralDTOs { }



    public class WalletDTO
    {
        [Required]
        public string Name { get; set; }
       
    

        [Required]
        public string ConnectedUserId { get; set; }
      
    }

    public class CurrencyDTO
    {
  
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }


        [Required]
        public string UserId { get; set; }
     
    }

    public class BankAccountDTO
    {
     
        [Required]
        public string AccountNumber { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

       
        [Required]
        public string CurrencyId { get; set; }

    }

    public class TransactionDTO
    {
        
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public int FromAccountId { get; set; }
      

        [Required]
        public int ToAccountId { get; set; }
  
        [Required]
        public int CurrencyId { get; set; }
    
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }


}
