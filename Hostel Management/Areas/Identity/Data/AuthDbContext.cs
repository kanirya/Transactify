using Hostel_Management.Areas.Identity.Data;
using Hostel_Management.Models;
using Hostel_Management.Models.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hostel_Management.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Wallets
        modelBuilder.Entity<Wallet>()
            .HasOne(w => w.Owner)
            .WithMany(u => u.Wallets)
            .HasForeignKey(w => w.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Wallet>()
            .HasOne(w => w.ConnectedUser)
            .WithMany()
            .HasForeignKey(w => w.ConnectedUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Currencies
        modelBuilder.Entity<Currency>()
            .HasOne(c => c.User)
            .WithMany(u => u.Currencies)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent user deletion from deleting currencies

        // BankAccounts
        modelBuilder.Entity<BankAccount>()
            .HasOne(b => b.User)
            .WithMany(u => u.BankAccounts)
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading user deletion

        modelBuilder.Entity<BankAccount>()
            .HasOne(b => b.Currency)
            .WithMany()
            .HasForeignKey(b => b.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Transactions
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromAccount)
            .WithMany()
            .HasForeignKey(t => t.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict); // Avoid deletion of accounts causing transaction loss

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToAccount)
            .WithMany()
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Wallet)
            .WithMany()
            .HasForeignKey(t => t.WalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Currency)
            .WithMany()
            .HasForeignKey(t => t.CurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Ensures transactions remain even if user is deleted
    }
}
