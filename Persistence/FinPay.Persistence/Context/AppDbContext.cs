using FinPay.Domain.Entity;
using FinPay.Domain.Entity.Paymet;
using FinPay.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPay.Persistence.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Tokeninfo> TokenInfos { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }
        public DbSet<AppTransaction> AppTransactions { get; set; }
        public DbSet<CardBalance> CardBalances { get; set; }
        public DbSet<PaypalTransaction> PaypalTransactions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppTransaction>()
                .HasOne(t => t.FromUser)
                .WithMany(u => u.SendTransactions)
                .HasForeignKey(t => t.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

          
        }

    }
}