using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ApplicationUserModel
{
    [Key]
    public string Id { get; set; }

    [Required]
    [StringLength(256)]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public ICollection<Account> Accounts { get; set; }
}

public class Currency
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(3)]
    public string Code { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }
}

public class Account
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string AccountName { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    //public ApplicationUser User { get; set; }

    [Required]
    public int CurrencyId { get; set; }

    [ForeignKey("CurrencyId")]
    public Currency Currency { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal Balance { get; set; }

    public ICollection<Transaction> TransactionsSent { get; set; }
    public ICollection<Transaction> TransactionsReceived { get; set; }
}

public class Transaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SenderAccountId { get; set; }

    [ForeignKey("SenderAccountId")]
    public Account SenderAccount { get; set; }

    [Required]
    public int ReceiverAccountId { get; set; }

    [ForeignKey("ReceiverAccountId")]
    public Account ReceiverAccount { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal AmountSent { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal AmountReceived { get; set; }

    [Required]
    public DateTime TransactionDate { get; set; }
}
