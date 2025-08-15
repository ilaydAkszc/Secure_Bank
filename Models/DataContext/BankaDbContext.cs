using Microsoft.EntityFrameworkCore;
using Secure_Bank.Models.Model;

namespace Secure_Bank.Models.DataContext
{

    public class BankaDbContext : DbContext
    {
        public BankaDbContext(DbContextOptions<BankaDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FullName).HasMaxLength(100);
                entity.Property(u => u.Email).HasMaxLength(100);
                entity.Property(u => u.Bio).HasMaxLength(1000);
                entity.HasIndex(u => u.Username).IsUnique();
            });

            // Account Configuration
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.AcId);
                entity.Property(a => a.AccountNumber).IsRequired().HasMaxLength(20);
                entity.Property(a => a.Balance).HasColumnType("decimal(18,2)");
                entity.Property(a => a.AccountType).HasMaxLength(20);
                entity.HasIndex(a => a.AccountNumber).IsUnique();

                entity.HasOne(a => a.User)
                      .WithMany(u => u.Accounts)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Transaction Configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.TrId);
                entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
                entity.Property(t => t.Description).HasMaxLength(500);
                entity.Property(t => t.TransactionType).HasMaxLength(20);

                entity.HasOne(t => t.FromAccount)
                      .WithMany(a => a.FromTransactions)
                      .HasForeignKey(t => t.FromAccountId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.ToAccount)
                      .WithMany(a => a.ToTransactions)
                      .HasForeignKey(t => t.ToAccountId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.User)
                      .WithMany(u => u.Transactions)
                      .HasForeignKey(t => t.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Users (Şifreler plain text - güvenlik açığı!)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "canank",
                    Password = "can123", // Plain text password!
                    FullName = "Canan Karatay",
                    Email = "canank@gmail.com",
                    Bio = "düzenli müşteri",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new User
                {
                    UserId = 2,
                    Username = "mehmetc",
                    Password = "meh123", // Plain text password!
                    FullName = "Mehmet Can",
                    Email = "mehc@gmail.com",
                    Bio = "yeni müşteri",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new User
                {
                    UserId = 3,
                    Username = "gülayç",
                    Password = "gül123", // Plain text password!
                    FullName = "Gülay Çiçek",
                    Email = "gülç@example.com",
                    Bio = "VIP müşteri",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                 new User
                 {
                     UserId = 4,
                     Username = "selimy",
                     Password = "sel123", // Plain text password!
                     FullName = "Selim Yıldırım",
                     Email = "selimy@example.com",
                     Bio = "VIP müşteri",
                     CreatedDate = new DateTime(2024, 1, 1)
                 }
            );

            // Seed Accounts
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AcId = 1,
                    UserId = 1,
                    AccountNumber = "1001",
                    Balance = 50000.00m,
                    AccountType = "Vadesiz",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Account
                {
                    AcId = 2,
                    UserId = 2,
                    AccountNumber = "1002",
                    Balance = 1500.75m,
                    AccountType = "Vadeli",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Account
                {
                    AcId = 3,
                    UserId = 2,
                    AccountNumber = "1003",
                    Balance = 5000.00m,
                    AccountType = "Tasarruf",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Account
                {
                    AcId = 4,
                    UserId = 3,
                    AccountNumber = "1004",
                    Balance = 25000.50m,
                    AccountType = "Vadesiz",
                    CreatedDate = new DateTime(2024, 1, 1)
                },
                new Account
                {
                    AcId = 5,
                    UserId = 4,
                    AccountNumber = "1005",
                    Balance = 15000.50m,
                    AccountType = "Vadesiz",
                    CreatedDate = new DateTime(2024, 1, 1)
                }
            );

            // Seed Transactions
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    TrId = 1,
                    FromAccountId = 2,
                    ToAccountId = 3,
                    UserId = 2,
                    Amount = 500.00m,
                    Description = "Depozito",
                    TransactionType = "Transfer",
                    TransactionDate = new DateTime(2024, 1, 1)
                },
                new Transaction
                {
                    TrId = 2,
                    FromAccountId = 4,
                    ToAccountId = 2,
                    UserId = 3,
                    Amount = 200.00m,
                    Description = "Borç",
                    TransactionType = "Transfer",
                    TransactionDate = new DateTime(2024, 1, 1)
                }
            );
        }
    }

}
