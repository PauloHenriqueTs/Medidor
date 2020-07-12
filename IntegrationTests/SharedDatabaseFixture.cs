using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication1.Data;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    public class SharedDatabaseFixture : IDisposable
    {
        public ApplicationDbContext dbContext;

        public SharedDatabaseFixture()
        {

            var config = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", false, true)
                        .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseSqlServer(config.GetConnectionString("DefaultConnection"));
            dbContext = new ApplicationDbContext(builder.Options);

        }

        public void Dispose()
        {

        }
    }
}