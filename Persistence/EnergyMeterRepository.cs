using Entities;
using Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.DAO
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
            var meterExist = await _dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            var MeterExist = await _dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
            if (meterExist == null && MeterExist == null)
            {
                if (energyMeter.Type == TypeOfEnergyMeter.House)
                {
                    var meter = new HouseEnergyMeter(energyMeter.SerialId, energyMeter.UserId, "0", true);

                    await _dbContext.HouseEnergyMeters.AddAsync(meter);
                    await _dbContext.SaveChangesAsync();
                }
                else if (energyMeter.Type == TypeOfEnergyMeter.Pole)
                {
                    var meter = new PoleEnergyMeter(energyMeter.SerialId, energyMeter.UserId, energyMeter.Meters);

                    if (meterExist == null)
                    {
                        await _dbContext.PoleEnergyMeters.AddAsync(meter);
                        await _dbContext.SaveChangesAsync();
                    }
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

        public async Task<EnergyMeter> GetById(string SerialId, string userId)
        {
            List<EnergyMeter> meters = new List<EnergyMeter>();
            var HouseMeters = await _dbContext.HouseEnergyMeters.Where(meters => meters.UserId == userId && meters.SerialId == SerialId).ToListAsync();
            foreach (var item in HouseMeters)
            {
                meters.Add(item.ToEnergyMeter());
            }
            var PoleMeters = await _dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).Where(meters => meters.UserId == userId && meters.SerialId == SerialId).ToListAsync();
            foreach (var item in PoleMeters)
            {
                meters.Add(item.ToEnergyMeter());
            }

            return meters.SingleOrDefault();
        }

        public async Task Delete(EnergyMeter energyMeter)
        {
            if (energyMeter.Type == TypeOfEnergyMeter.House)
            {
                var meter = new HouseEnergyMeter(energyMeter.SerialId, energyMeter.UserId, energyMeter.Count, energyMeter.SwitchState);
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

        public async Task DeleteById(string serialId)
        {
            var meterExist = await _dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == serialId);
            if (meterExist != null)
            {
                if (_dbContext.Entry(meterExist).State == EntityState.Detached)
                {
                    _dbContext.HouseEnergyMeters.Attach(meterExist);
                }
                _dbContext.HouseEnergyMeters.Remove(meterExist);
                await _dbContext.SaveChangesAsync();
            }
            var MeterExist = await _dbContext.PoleEnergyMeters.Include(meter => meter.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == serialId);
            if (MeterExist != null)
            {
                if (_dbContext.Entry(MeterExist).State == EntityState.Detached)
                {
                    _dbContext.PoleEnergyMeters.Attach(MeterExist);
                }
                _dbContext.PoleEnergyMeters.Remove(MeterExist);
                await _dbContext.SaveChangesAsync();
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
                var meter = new HouseEnergyMeter(energyMeter.SerialId, energyMeter.UserId, energyMeter.Count, energyMeter.SwitchState);
                var meterExist = await _dbContext.PoleEnergyMeters.Include(meters => meters.MeterOfPoleEnergyMeters).FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
                if (meterExist != null)
                {
                    if (_dbContext.Entry(meterExist).State == EntityState.Detached)
                    {
                        _dbContext.PoleEnergyMeters.Attach(meterExist);
                    }
                    _dbContext.PoleEnergyMeters.Remove(meterExist);
                    await _dbContext.HouseEnergyMeters.AddAsync(meter);
                    await _dbContext.SaveChangesAsync();
                }
                var meterHouseExist = await _dbContext.HouseEnergyMeters.FirstOrDefaultAsync(m => m.SerialId == energyMeter.SerialId);
                if (meterHouseExist != null)
                {
                    if (_dbContext.Entry(meterHouseExist).State == EntityState.Detached)
                    {
                        _dbContext.HouseEnergyMeters.Attach(meterHouseExist);
                    }
                    meterHouseExist.Count = energyMeter.Count;
                    meterHouseExist.SwitchState = energyMeter.SwitchState;
                    await _dbContext.SaveChangesAsync();
                }

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