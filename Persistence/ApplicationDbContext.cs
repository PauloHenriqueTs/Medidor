using System;
using System.Collections.Generic;
using System.Text;
using Command;
using Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.DAO;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        internal DbSet<MeterOfPoleDao> MeterOfPoleDaos { get; set; }

        internal DbSet<EnergyMeterDao> EnergyMetersDaos { get; set; }

        internal DbSet<MeterCommand> MeterCommands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MeterCommand>()
                        .Property(c => c.Type)
                        .HasConversion(
                            t => t.ToString(),
                            t => (MeterCommandType)Enum.Parse(typeof(MeterCommandType), t));
        }
    }
}