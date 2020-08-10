using Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class CommandRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommandRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MeterCommand>> Get(string userId)
        {
            var commands = await _dbContext.MeterCommands.Where(c => c.UserId == userId).AsNoTracking().ToListAsync();
            return commands;
        }

        public async Task Create(MeterCommand command)
        {
            try
            {
                await _dbContext.AddAsync(command);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        public async Task CreateSwitchCommand(string serialId, string userId)
        {
            try
            {
                var command = new MeterCommand(MeterCommandType.SwitchCommand, serialId, userId);
                await _dbContext.AddAsync(command);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        public async Task GetCountCommand(string serialId, string userId)
        {
            try
            {
                var command = new MeterCommand(MeterCommandType.GetCountCommand, serialId, userId);
                await _dbContext.AddAsync(command);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }
    }
}