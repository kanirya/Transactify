using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hostel_Management.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

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
    public string AccountName { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Balance must be 0 or greater.")]
    public decimal Balance { get; set; }
    [Required]
    [ForeignKey("UserId")]
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    [Required]
    [ForeignKey("CurrencyId")]
    public int CurrencyId { get; set; }
    public Currency Currency { get; set; }


  
}

public class Transaction
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SenderAccountId { get; set; }

    [Required]
    public string ReceiverAccountId { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal AmountSent { get; set; }

    [Required]
    [DataType(DataType.Currency)]
    public decimal AmountReceived { get; set; }

    [Required]
    public DateTime TransactionDate { get; set; }
}