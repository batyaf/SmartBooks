using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace QBCustomer.Models
{
    public class SmartBooksContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public SmartBooksContext(DbContextOptions<SmartBooksContext> options) : base(options) { }
        public DbSet<CustomerToken> CustomerTokens { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<QbContactInfo> QbContactInfos { get; set; }

        public DbSet<QbAddress> qbAddresses { get; set; }

        public DbSet<SBUser> sbUsers { get; set; }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerModel>()
            .HasOne(c => c.BillAddr)
            .WithOne()
            .HasForeignKey<CustomerModel>(c => c.BillAddrId);

            modelBuilder.Entity<CustomerModel>()
                .HasOne(c => c.PrimaryPhone)
                .WithOne()
                .HasForeignKey<CustomerModel>(c => c.PrimaryPhoneId);

            modelBuilder.Entity<CustomerModel>()
           .HasOne(c => c.PrimaryEmailAddr)
           .WithOne()
           .HasForeignKey<CustomerModel>(c => c.PrimaryEmailAddrId);

            modelBuilder.Entity<CustomerToken>()
             .HasOne(ct => ct.User)
             .WithMany()
             .HasForeignKey(ct => ct.UsrId);

            modelBuilder.Entity<CustomerModel>()
                .HasOne(cm => cm.User)
                .WithMany()
                .HasForeignKey(cm => cm.UsrId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
