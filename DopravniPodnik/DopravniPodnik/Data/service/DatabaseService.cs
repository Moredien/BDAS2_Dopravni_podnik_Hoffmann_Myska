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

    public int GetUserTypeId(string userType)
    {
        var userTypeId = -1; // Default hodnota
        using var connection = _context.Database.GetDbConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT ID_TYP_UZIVATELE FROM ST67028.TYPY_UZIVATELE WHERE NAZEV = :userType";
        
        var parameter = command.CreateParameter();
        parameter.ParameterName = "userType";
        parameter.Value = userType;
        command.Parameters.Add(parameter);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            userTypeId = reader.GetInt32(reader.GetOrdinal("ID_TYP_UZIVATELE"));
        }

        return userTypeId;
    }

}