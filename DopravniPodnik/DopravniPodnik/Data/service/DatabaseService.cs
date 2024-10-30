using Microsoft.EntityFrameworkCore;

namespace DopravniPodnik.Data.service;

public class DatabaseService(OracleDbContext context)
{
    public bool TestConnection()
    {
        try
        {
            //Test jestli je mozne se pripojit k db
            context.Database.OpenConnection();
            context.Database.CloseConnection();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection test failed: {ex.Message}");
            return false;
        }
    }
}