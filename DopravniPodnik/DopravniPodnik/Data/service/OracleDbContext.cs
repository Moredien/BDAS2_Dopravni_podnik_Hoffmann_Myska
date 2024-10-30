using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace DopravniPodnik.Data.service;

public class OracleDbContext(DbContextOptions<OracleDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("ORACLE_DB_CONNECTION");
        
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Oracle DB connection string not found");
        }
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseOracle();
        }
    }
}