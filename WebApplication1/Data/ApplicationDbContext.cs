using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EnergyMeter> EnergyMeters { get; set; }

        public DbSet<MeterOfPole> MeterOfPole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EnergyMeter>()
                .HasOne(e => e.user)
               .WithMany(u => u.EnergyMeters)
               .HasForeignKey(e => e.userId);

            modelBuilder.Entity<MeterOfPole>()
                .HasOne(m => m.poleMeter)
                .WithMany(p => p.EnergyMeters)
                .HasForeignKey(m => m.poleSerialId);
        }
    }
}