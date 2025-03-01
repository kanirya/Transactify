using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Hostel_Management.Models.Model;
using Microsoft.AspNetCore.Identity;

namespace Hostel_Management.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Column(TypeName ="nvarchar(100)")]
    public string Name { get; set; }
    public virtual List<Wallet> Wallets { get; set; } = new List<Wallet>();
    public virtual List<Currency> Currencies { get; set; } = new List<Currency>();
    public virtual List<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();



}

