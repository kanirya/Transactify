namespace Hostel_Management.Models.DTOs
{
    public class AccountDTO
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public string UserId { get; set; } // Foreign Key
        public int CurrencyId { get; set; } // Foreign Key
    }

}
