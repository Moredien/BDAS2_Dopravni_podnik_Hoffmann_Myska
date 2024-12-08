using System.Reflection;
using DopravniPodnik.Utils;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.Data.service;

public class DatabaseService
{
    private readonly OracleDbContext _context = OracleDbContext.Instance;
    private readonly Logger _logger = App.LoggerInstance;

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
            _logger.Message($"Database connection test failed: {ex.Message}").Error().Log();
            return false;
        }
    }

    public void ExecuteDbCall(ProcedureCallWrapper procedure, out string error)
    {
        var connection = _context.Database.GetDbConnection() as OracleConnection;
        if (connection == null)
        {
            _logger.Message("Database connection is not type of OracleConnection").Error().Log();
            error = "Database connection is not type of OracleConnection";
            return;
        }
    
        try
        {
            connection.Open();

            using var command = new OracleCommand(procedure.Query, connection);

            if (procedure.Parameters.Count != 0)
            {
                command.Parameters.AddRange(procedure.Parameters.ToArray());
            }
            
            //command.ExecuteNonQuery();
            error = string.Empty;
        }
        catch (Exception e)
        {
            _logger.Exception(e).Log();
            error = e.Message;
        }
        finally
        {
            connection.Close();
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
    
    public List<TDto> FetchData<TDto>(string query) where TDto : new()
    {
        var results = new List<TDto>();
        
        var connection = _context.Database.GetDbConnection() as OracleConnection;

        if (connection == null)
        {
            _logger.Message("Database connection is not type of OracleConnection").Error().Log();
            return results;
        }

        try
        {
            connection.Open();

            using var command = new OracleCommand(query, connection);
            using var reader = command.ExecuteReader();
            var properties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (reader.Read())
            {
                var obj = new TDto();

                foreach (var property in properties)
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnNameAttribute>();
                    var columnName = columnAttribute?.Name ?? property.Name;

                    if (!reader.HasColumn(columnName))
                        continue;

                    var value = reader[columnName];
                    if (value == DBNull.Value) value = null;

                    property.SetValue(obj, ConvertValue(value, property.PropertyType));
                }

                results.Add(obj);
            }
        }
        catch (Exception e)
        {
            _logger.Exception(e).Log();
            return [];
        }
        finally
        {
            connection.Close();
        }
        
        return results;
    }
    
    public List<TDto> FetchDataParam<TDto>(ProcedureCallWrapper procedure) where TDto : new()
    {
        var results = new List<TDto>();

        var connection = _context.Database.GetDbConnection() as OracleConnection;

        if (connection == null)
        {
            _logger.Message("Database connection is not type of OracleConnection").Error().Log();
            return results;
        }

        try
        {
            connection.Open();

            using var command = new OracleCommand(procedure.Query, connection);
        
            command.Parameters.AddRange(procedure.Parameters.ToArray());

            using var reader = command.ExecuteReader();
            var properties = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            while (reader.Read())
            {
                var obj = new TDto();

                foreach (var property in properties)
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnNameAttribute>();
                    var columnName = columnAttribute?.Name ?? property.Name;

                    if (!reader.HasColumn(columnName))
                        continue;

                    var value = reader[columnName];
                    if (value == DBNull.Value) value = null;

                    property.SetValue(obj, ConvertValue(value, property.PropertyType));
                }

                results.Add(obj);
            }
        }
        catch (Exception e)
        {
            _logger.Exception(e).Log();
            return [];
        }
        finally
        {
            connection.Close();
        }

        return results;
    }
    
    private static object? ConvertValue(object? value, Type targetType)
    {
        if (value == null || targetType.IsInstanceOfType(value))
            return value;

        if (!targetType.IsGenericType || targetType.GetGenericTypeDefinition() != typeof(Nullable<>))
            return Convert.ChangeType(value, targetType);
        
        var underlyingType = Nullable.GetUnderlyingType(targetType);
        return Convert.ChangeType(value, underlyingType!);
    }
}
