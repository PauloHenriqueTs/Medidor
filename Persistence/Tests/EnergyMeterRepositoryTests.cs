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
    }
}