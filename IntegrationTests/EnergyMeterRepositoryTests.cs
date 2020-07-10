using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Data.Repository;
using WebApplication1.Entities;
using WebApplication1.ValueObjects;
using Xunit;

namespace IntegrationTests
{
    public class EnergyMeterRepositoryTests
    {
        private ApplicationDbContext dbContext;
        private EnergyMeterRepository repository;

        public EnergyMeterRepositoryTests()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseSqlServer($"Data Source =localhost/SQLEXPRESS; Initial Catalog = Energy; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            dbContext = new ApplicationDbContext(builder.Options);
            repository = new EnergyMeterRepository(dbContext);
        }

        [Fact]
        public async Task QueryMonstersFromSqlTest()
        {
            dbContext.HouseEnergyMeters.ToList();
        }
    }
}