namespace Hostel_Management.Models.DTOs
{
    public class AccountDTO
    {
        public string Id { get; set; }
        public string AccountName { get; set; }
        public decimal Balance { get; set; }
        public int CurrencyId { get; set; } // Foreign Key
    }

}
