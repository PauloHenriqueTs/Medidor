using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.DAO;
using WebApplication1.Entities;
using WebApplication1.ValueObjects;

namespace WebApplication1.Data.Repository
{
    public class EnergyMeterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EnergyMeterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(EnergyMeter energyMeter)
        {
            if (energyMeter.Type == TypeOfEnergyMeter.House)
            {
                var meter = new HouseEnergyMeter(energyMeter.SerialId, energyMeter.UserId);
                var meterExist = await _dbContext.HouseEnergyMeters.FirstAsync(m => m.SerialId == meter.SerialId);
                if (meterExist == null)
                {
                    await _dbContext.HouseEnergyMeters.AddAsync(meter);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else if (energyMeter.Type == TypeOfEnergyMeter.Pole)
            {
                var meter = new PoleEnergyMeter(energyMeter.SerialId, energyMeter.UserId, energyMeter.Meters);
                if (await _dbContext.PoleEnergyMeters.FindAsync(meter) == null)
                {
                    await _dbContext.PoleEnergyMeters.AddAsync(meter);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<List<EnergyMeter>> Get()
        {
            List<EnergyMeter> meters = new List<EnergyMeter>();
            var HouseMeters = await _dbContext.HouseEnergyMeters.ToListAsync();
            foreach (var item in HouseMeters)
            {
                meters.Add(item.ToEnergyMeter());
            }
            var PoleMeters = await _dbContext.PoleEnergyMeters.ToListAsync();
            foreach (var item in PoleMeters)
            {
                meters.Add(item.ToEnergyMeter());
            }

            return meters;
        }
    }
}