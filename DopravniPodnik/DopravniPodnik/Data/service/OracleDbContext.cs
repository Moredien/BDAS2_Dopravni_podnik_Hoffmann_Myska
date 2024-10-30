using Microsoft.EntityFrameworkCore;

namespace DopravniPodnik.Data.service;

public class OracleDbContext(DbContextOptions<OracleDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseOracle("User Id=st67028;Password=SQLP455w0rd;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=FEI-SQL3.UPCEUCEBNY.CZ)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=BDAS)));");
        }
    }
}