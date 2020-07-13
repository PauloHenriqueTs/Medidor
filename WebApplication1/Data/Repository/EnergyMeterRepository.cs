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
                var meterExist = await _dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == meter.SerialId);
                if (meterExist == null)
                {
                    await _dbContext.HouseEnergyMeters.AddAsync(meter);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else if (energyMeter.Type == TypeOfEnergyMeter.Pole)
            {
                var meter = new PoleEnergyMeter(energyMeter.SerialId, energyMeter.UserId, energyMeter.Meters);
                var meterExist = await _dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == meter.SerialId);
                if (meterExist == null)
                {
                    await _dbContext.PoleEnergyMeters.AddAsync(meter);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<List<EnergyMeter>> Get(string userId)
        {
            List<EnergyMeter> meters = new List<EnergyMeter>();
            var HouseMeters = await _dbContext.HouseEnergyMeters.Where(meters => meters.UserId == userId).ToListAsync();
            foreach (var item in HouseMeters)
            {
                meters.Add(item.ToEnergyMeter());
            }
            var PoleMeters = await _dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).Where(meters => meters.UserId == userId).ToListAsync();
            foreach (var item in PoleMeters)
            {
                meters.Add(item.ToEnergyMeter());
            }

            return meters;
        }

        public async Task Delete(EnergyMeter energyMeter)
        {
            if (energyMeter.Type == TypeOfEnergyMeter.House)
            {
                var meter = new HouseEnergyMeter(energyMeter.SerialId, energyMeter.UserId);
                var meterExist = await _dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == meter.SerialId);
                if (meterExist != null)
                {
                    if (_dbContext.Entry(meterExist).State == EntityState.Detached)
                    {
                        _dbContext.HouseEnergyMeters.Attach(meterExist);
                    }
                    _dbContext.HouseEnergyMeters.Remove(meterExist);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else if (energyMeter.Type == TypeOfEnergyMeter.Pole)
            {
                var meter = new PoleEnergyMeter(energyMeter.SerialId, energyMeter.UserId, energyMeter.Meters);
                var meterExist = await _dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == meter.SerialId);
                if (meterExist != null)
                {
                    if (_dbContext.Entry(meterExist).State == EntityState.Detached)
                    {
                        _dbContext.PoleEnergyMeters.Attach(meterExist);
                    }
                    _dbContext.PoleEnergyMeters.Remove(meterExist);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> SerialIdExist(string SerialId)
        {
            var PoleMeterExist = await _dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == SerialId);
            var HouseMeterExist = await _dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == SerialId);
            if (PoleMeterExist != null)
            {
                return true;
            }

            else if (HouseMeterExist != null)
            {
                return true;
            }

            return false;

        }

        public async Task Update(EnergyMeter energyMeter)
        {
            if (energyMeter.Type == TypeOfEnergyMeter.House)
            {
                var meter = new HouseEnergyMeter(energyMeter.SerialId, energyMeter.UserId);
                var meterExist = await _dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
                if (meterExist != null)
                {
                    if (_dbContext.Entry(meterExist).State == EntityState.Detached)
                    {
                        _dbContext.PoleEnergyMeters.Attach(meterExist);
                    }
                    _dbContext.PoleEnergyMeters.Remove(meterExist);
                    await _dbContext.SaveChangesAsync();
                }
                await _dbContext.HouseEnergyMeters.AddAsync(meter);
                await _dbContext.SaveChangesAsync();
            }

            else if (energyMeter.Type == TypeOfEnergyMeter.Pole)
            {
                var meter = new PoleEnergyMeter(energyMeter.SerialId, energyMeter.UserId, energyMeter.Meters);
                var meterExist = await _dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
                if (meterExist != null)
                {
                    if (_dbContext.Entry(meterExist).State == EntityState.Detached)
                    {
                        _dbContext.HouseEnergyMeters.Attach(meterExist);
                    }
                    _dbContext.HouseEnergyMeters.Remove(meterExist);
                    await _dbContext.SaveChangesAsync();
                }
                await _dbContext.PoleEnergyMeters.AddAsync(meter);
                await _dbContext.SaveChangesAsync();

            }




        }


    }
}