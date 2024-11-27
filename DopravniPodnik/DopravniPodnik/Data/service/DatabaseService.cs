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

    public bool CallDbProcedure(string procedure, OracleParameter[] parameters, out string? errorMessage)
    {
        errorMessage = string.Empty;

        var connection = _context.Database.GetDbConnection() as OracleConnection;
        if (connection == null)
        {
            _logger.Message("Database connection is not type of OracleConnection").Error().Log();
            return false;
        }
    
        try
        {
            connection.Open();

            using var command = new OracleCommand(procedure, connection);
            
            command.Parameters.AddRange(parameters);
            var result = command.ExecuteNonQuery();

            var errorParam = parameters.FirstOrDefault(p => p.Direction == System.Data.ParameterDirection.Output);
            if (errorParam != null && errorParam.Value != DBNull.Value)
            {
                errorMessage = errorParam.Value.ToString();
            }

            return errorMessage == "null";
        }
        catch (Exception e)
        {
            _logger.Exception(e).Log();
            errorMessage = e.Message;
            return false;
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
    
    public void UpdateTable<T>(string tableName, string whereCondition, T data) where T : class
    {
        ArgumentNullException.ThrowIfNull(data);

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (properties.Length == 0)
        {
            _logger.Message($"The data object {nameof(data)} has no properties.").Error().Log();
            return;
        }

        var columns = new List<string>();
        var values = new List<object>();

        foreach (var property in properties)
        {
            var columnAttribute = property.GetCustomAttribute<ColumnNameAttribute>();
            var columnName = columnAttribute?.Name ?? property.Name;

            var value = property.GetValue(data);
            if (value == null) continue; // TODO jak resit null ?
            
            columns.Add(columnName);
            values.Add(value);
        }

        if (columns.Count == 0)
        {
            _logger.Message("There are no data to update").Warning().Log();
            return;
        }

        var setClause = string.Join(", ", columns.Select((col, index) => $"{col} = :param{index}"));
        var sql = $"UPDATE {tableName} SET {setClause} WHERE {whereCondition}";

        using var connection = _context.Database.GetDbConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = sql;

        for (var i = 0; i < values.Count; i++)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = $"param{i}";
            parameter.Value = values[i]; 
            command.Parameters.Add(parameter);
        }

        try
        {
            var affectedRows = command.ExecuteNonQuery();
            _logger.Message($"Updated {affectedRows} rows in {tableName}.").Info().Log();
        }
        catch (Exception ex)
        {
            _logger.Exception(ex).Log();
            throw;
        }
        finally
        {
            connection.Close();
        }
    }
    
    public void InsertIntoTable<T>(string tableName, T data) where T : class
    {
        ArgumentNullException.ThrowIfNull(data);

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (properties.Length == 0)
        {
            _logger.Message($"The data object {nameof(data)} has no properties.").Error().Log();
            return;
        }
        
        var columns = new List<string>();
        var parameters = new List<string>();
        var values = new List<object>();

        foreach (var property in properties)
        {
            var columnAttribute = property.GetCustomAttribute<ColumnNameAttribute>();
            var columnName = columnAttribute?.Name ?? property.Name;

            var value = property.GetValue(data);
            if (value == null) continue; // TODO null ???
            
            columns.Add(columnName);
            parameters.Add($":param{values.Count}"); //parameter placeholder
            values.Add(value);
        }

        if (columns.Count == 0)
        {
            _logger.Message("There are no data to insert").Warning().Log();
            return;
        }

        var columnList = string.Join(", ", columns);
        var parameterList = string.Join(", ", parameters);
        var sql = $"INSERT INTO {tableName} ({columnList}) VALUES ({parameterList})";

        using var connection = _context.Database.GetDbConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = sql;

        for (var i = 0; i < values.Count; i++)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = $"param{i}";
            parameter.Value = values[i];
            command.Parameters.Add(parameter);
        }
        try
        {
            var affectedRows = command.ExecuteNonQuery();
            _logger.Message($"Inserted {affectedRows} rows into {tableName}.").Info().Log();
        }
        catch (Exception ex)
        {
            _logger.Exception(ex).Log();
            throw;
        }
        finally
        {
            connection.Close();
        }
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
