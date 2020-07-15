using System;
using System.Collections.Generic;
using System.Text;
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

        internal DbSet<MeterOfPoleEnergyMeter> MeterOfPoleEnergyMeters { get; set; }

        internal DbSet<PoleEnergyMeter> PoleEnergyMeters { get; set; }

        internal DbSet<HouseEnergyMeter> HouseEnergyMeters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}