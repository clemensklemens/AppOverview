using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOverview.Data.Models
{
    public partial class AppOverviewContext : DbContext
    {
        private readonly string _connectionString;
        public AppOverviewContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Detect provider from connection string
                if (_connectionString.Contains("Filename=", StringComparison.OrdinalIgnoreCase) ||
                    _connectionString.Contains(".db", StringComparison.OrdinalIgnoreCase) ||
                    (_connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase) && _connectionString.Contains(".db")))
                {
                    optionsBuilder.UseSqlite(_connectionString);
                }
                else if (_connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase) ||
                         (_connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase)))
                {
                    optionsBuilder.UseSqlServer(_connectionString);
                }
                else
                {
                    throw new InvalidOperationException("Unknown database provider in connection string.");
                }
            }
        }
    }
}
