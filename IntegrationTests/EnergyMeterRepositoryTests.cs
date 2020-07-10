using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Data.DAO;
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
            var config = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", false, true)
                        .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase("test");
            dbContext = new ApplicationDbContext(builder.Options);
            repository = new EnergyMeterRepository(dbContext);
        }

        [Fact]
        public async Task QueryMonstersFromSqlTest()
        {
            var energyMeter = new EnergyMeter("1", "1", TypeOfEnergyMeter.House, null);

            await repository.Create(energyMeter);

            var dbResult = await dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            var result = dbResult.ToEnergyMeter();
            Assert.Equal(energyMeter, result);
        }
    }
}