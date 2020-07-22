using Entities;
using Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using Persistence.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace Persistence.Tests
{
    public class EnergyMeterRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        private ApplicationDbContext dbContext;
        private EnergyMeterRepository repository;

        public EnergyMeterRepositoryTests(SharedDatabaseFixture sharedDatabase)
        {
            dbContext = sharedDatabase.dbContext;
            repository = new EnergyMeterRepository(dbContext);
        }

        [Fact]
        public async Task CreateHouseEnergyMeter()
        {
            var energyMeter = new EnergyMeter("1", "1", TypeOfEnergyMeter.House, null);

            await repository.Create(energyMeter);

            var dbResult = await dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            var result = dbResult.ToEnergyMeter();
            Assert.Equal(energyMeter, result);
        }

        [Fact]
        public async Task CreatePoleEnergyMeter()
        {
            var meters = new List<MeterOfPole>();
            meters.Add(new MeterOfPole("1"));
            meters.Add(new MeterOfPole("2"));
            meters.Add(new MeterOfPole("3"));

            var energyMeter = new EnergyMeter("2", "1", TypeOfEnergyMeter.Pole, meters);

            await repository.Create(energyMeter);

            var dbResult = await dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            var result = dbResult.ToEnergyMeter();
            Assert.Equal(energyMeter, result);
        }

        [Fact]
        public async Task GetAllEnergyMeter()
        {
            var userId = "3";
            var meters = new List<MeterOfPole>();
            meters.Add(new MeterOfPole("63"));
            meters.Add(new MeterOfPole("73"));
            meters.Add(new MeterOfPole("83"));
            var GetData = new List<EnergyMeter>();
            GetData.Add(new EnergyMeter("33", userId, TypeOfEnergyMeter.House, null));
            GetData.Add(new EnergyMeter("43", userId, TypeOfEnergyMeter.House, null));
            GetData.Add(new EnergyMeter("53", userId, TypeOfEnergyMeter.House, null));
            GetData.Add(new EnergyMeter("63", userId, TypeOfEnergyMeter.Pole, meters));

            foreach (var item in GetData)
            {
                await repository.Create(item);
            }
            var DbResult = await repository.Get(userId);

            Assert.Equal(GetData, DbResult);
        }

        [Fact]
        public async Task DeleteHouseEnergyMeter()
        {
            var userId = "4";
            var energyMeter = new EnergyMeter("14", userId, TypeOfEnergyMeter.House, null);
            await repository.Create(energyMeter);

            await repository.Delete(energyMeter);
            var AfterInsertDb = await dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            Assert.Null(AfterInsertDb);
        }

        [Fact]
        public async Task PoleHouseEnergyMeter()
        {
            var userId = "4";
            var meters = new List<MeterOfPole>();
            meters.Add(new MeterOfPole("144"));
            meters.Add(new MeterOfPole("244"));
            meters.Add(new MeterOfPole("344"));
            var energyMeter = new EnergyMeter("24", userId, TypeOfEnergyMeter.Pole, meters);
            await repository.Create(energyMeter);

            await repository.Delete(energyMeter);
            var AfterInsertDb = await dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            Assert.Null(AfterInsertDb);
        }

        [Fact]
        public async Task UpdateHouseEnergyMeter()
        {
            var userId = "5";
            var meters = new List<MeterOfPole>();
            meters.Add(new MeterOfPole("5441"));
            meters.Add(new MeterOfPole("5442"));
            meters.Add(new MeterOfPole("5443"));
            var energyMeter = new EnergyMeter("15", userId, TypeOfEnergyMeter.Pole, meters);

            await repository.Create(energyMeter);
            energyMeter = new EnergyMeter("15", userId, TypeOfEnergyMeter.House, null);
            await repository.Update(energyMeter);

            var AfterUpdateDb = await dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            Assert.Equal(energyMeter, AfterUpdateDb.ToEnergyMeter());
        }

        [Fact]
        public async Task UpdatePoleEnergyMeter()
        {
            var userId = "6";
            var meters = new List<MeterOfPole>();
            meters.Add(new MeterOfPole("6441"));
            meters.Add(new MeterOfPole("6442"));
            meters.Add(new MeterOfPole("6443"));
            var energyMeter = new EnergyMeter("16", userId, TypeOfEnergyMeter.House, null);

            await repository.Create(energyMeter);
            energyMeter = new EnergyMeter("16", userId, TypeOfEnergyMeter.Pole, meters);
            await repository.Update(energyMeter);

            var AfterUpdateDb = await dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            Assert.Equal(energyMeter, AfterUpdateDb.ToEnergyMeter());
        }
    }
}