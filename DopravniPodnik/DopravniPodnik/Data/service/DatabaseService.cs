using DopravniPodnik.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DopravniPodnik.Data.service;

public class DatabaseService
{
    private readonly OracleDbContext _context = OracleDbContext.Instance;

    public bool TestConnection()
    {
        try
        {
            //Test jestli je mozne se pripojit k db
            _context.Database.OpenConnection();
            _context.Database.CloseConnection();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection test failed: {ex.Message}");
            return false;
        }
    }
}