using Command;
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
            try
            {
                var dao = new EnergyMeterDao(energyMeter);
                await _dbContext.AddAsync(dao);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        public async Task<List<EnergyMeter>> Get(string userId)
        {
            var AllEnergyMeters = new List<EnergyMeter>();
            var daoWithMeterOfPole = await _dbContext.EnergyMetersDaos.Include(m => m.MeterOfPoleDao).Where(m => m.UserId == userId).ToListAsync();
            var daoWithoutMeterOfPole = await _dbContext.EnergyMetersDaos.Where(m => m.UserId == userId && m.MeterOfPoleDao == null).ToListAsync();
            foreach (var item in daoWithMeterOfPole)
            {
                AllEnergyMeters.Add(item.ToEnergyMeter());
            }
            foreach (var item in daoWithoutMeterOfPole)
            {
                AllEnergyMeters.Add(item.ToEnergyMeter());
            }
            return AllEnergyMeters;
        }

        public async Task<EnergyMeter> GetById(string SerialId, string UserId)
        {
            var dao = await _dbContext.EnergyMetersDaos.FirstOrDefaultAsync(m => m.SerialId == SerialId && m.UserId == UserId);
            return dao.ToEnergyMeter();
        }

        public async Task Delete(EnergyMeter energyMeter)
        {
            var dao = await _dbContext.EnergyMetersDaos.FindAsync(energyMeter.SerialId);
            if (dao != null)
            {
                if (_dbContext.Entry(dao).State == EntityState.Detached)
                {
                    _dbContext.EnergyMetersDaos.Attach(dao);
                }
                _dbContext.EnergyMetersDaos.Remove(dao);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteById(string SerialId)
        {
            var dao = await _dbContext.EnergyMetersDaos.FindAsync(SerialId);
            if (dao != null)
            {
                if (_dbContext.Entry(dao).State == EntityState.Detached)
                {
                    _dbContext.EnergyMetersDaos.Attach(dao);
                }
                _dbContext.EnergyMetersDaos.Remove(dao);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> SerialIdExist(string SerialId)
        {
            var dao = await _dbContext.EnergyMetersDaos.FindAsync(SerialId);
            if (dao == null)
                return false;
            return true;
        }

        public async Task Update(EnergyMeter energyMeter)
        {
            var dao = await _dbContext.EnergyMetersDaos.FindAsync(energyMeter.SerialId);
            if (dao != null)
            {
                var newDao = new EnergyMeterDao(energyMeter);
                dao.Type = newDao.Type;
                if (newDao.Count != null)
                    dao.Count = newDao.Count;
                if (newDao.SwitchState != null)
                    dao.SwitchState = newDao.SwitchState;
                dao.MeterOfPoleDao = newDao.MeterOfPoleDao;

                _dbContext.Entry(dao).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}