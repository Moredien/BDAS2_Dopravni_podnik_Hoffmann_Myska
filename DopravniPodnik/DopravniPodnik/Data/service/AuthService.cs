using System.Security.Cryptography;
using System.Text;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DopravniPodnik.Data.service;

public class AuthService
{
    private readonly OracleDbContext _context = OracleDbContext.Instance;
    private readonly DatabaseService _databaseService = new();
    private readonly Logger _logger = App.LoggerInstance;
    
    private const int NumberOfIterations = 10000;
    private const string InsertAddressCommand = @"
            INSERT INTO ST67028.ADRESY (MESTO, ULICE, CISLO_POPISNE)
            VALUES ({0}, {1}, {2})
            RETURNING ID_ADRESY INTO :outputId";
    private const string InsertUserCommand = @"
            INSERT INTO ST67028.UZIVATELE 
            (UZIVATELSKE_JMENO, HESLO, JMENO, PRIJMENI, CAS_ZALOZENI, DATUM_NAROZENI, ID_TYP_UZIVATELE, ID_ADRESY)
            VALUES (:UzivatelskeJmeno, :Heslo, :Jmeno, :Prijmeni, :CasZalozeni, :DatumNarozeni, :IdTypUzivatele, :IdAdresy)";

    public UserRegistrationResult RegisterUser(UzivatelDTO uzivatel)
    {
        try
        {
            Uzivatele? existingUser = _context.Uzivatele
                .FromSqlRaw("SELECT * FROM ST67028.UZIVATELE WHERE UZIVATELSKE_JMENO = {0}", uzivatel.uzivatelske_jmeno)
                .FirstOrDefault();

            if (existingUser != null) return UserRegistrationResult.AlreadyRegistered;

            var hashedPassword = HashPassword(uzivatel.heslo);

            var userTypeId = _databaseService.GetUserTypeId("admin");
            if (userTypeId == -1)
            {
                _logger.Message("ID_TYP_UZIVATELE not found").Error().Log();
                return UserRegistrationResult.Failed;
            }
        
            var idAdresyParam = new Oracle.ManagedDataAccess.Client.OracleParameter
            {
                ParameterName = "outputId",
                DbType = System.Data.DbType.Int32,
                Direction = System.Data.ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(InsertAddressCommand, uzivatel.mesto, uzivatel.ulice, uzivatel.cislo_popisne, idAdresyParam);
            int idAdresy = (int)idAdresyParam.Value;
        
            object[] parameters =
            [
                new Oracle.ManagedDataAccess.Client.OracleParameter("UzivatelskeJmeno", uzivatel.uzivatelske_jmeno),
                new Oracle.ManagedDataAccess.Client.OracleParameter("Heslo", hashedPassword),
                new Oracle.ManagedDataAccess.Client.OracleParameter("Jmeno", uzivatel.jmeno),
                new Oracle.ManagedDataAccess.Client.OracleParameter("Prijmeni", uzivatel.prijmeni),
                new Oracle.ManagedDataAccess.Client.OracleParameter("CasZalozeni", DateTime.Now),
                new Oracle.ManagedDataAccess.Client.OracleParameter("DatumNarozeni", uzivatel.datum_narozeni),
                new Oracle.ManagedDataAccess.Client.OracleParameter("IdTypUzivatele", userTypeId),
                new Oracle.ManagedDataAccess.Client.OracleParameter("IdAdresy", idAdresy)
            ];
        
            var result = _context.Database.ExecuteSqlRaw(InsertUserCommand, parameters);

            if (result != 1)
            {
                _logger.Message("User wasn't inserted into DB").Error().Log();
                return UserRegistrationResult.Failed;
            }

            _logger.Message("User was inserted into DB").Info().Log();
            return UserRegistrationResult.Success;
        }
        catch (Exception e)
        {
            _logger.Exception(e).Log();
            return UserRegistrationResult.Failed;
        }
    }
    
    public UserLoginResult LoginUser(string uzivatelskeJmeno, string heslo)
    {
        try
        {
            var user = _context.Uzivatele
                .FromSqlInterpolated($"SELECT * FROM ST67028.UZIVATELE WHERE UZIVATELSKE_JMENO = {uzivatelskeJmeno}")
                .FirstOrDefault();

            if (user == null)
            {
                _logger.Message($"User with name: {uzivatelskeJmeno} doesn't exist").Info().Log();
                return UserLoginResult.NotRegistered;
            }

            if (!ValidatePassword(heslo, user.Heslo)) return UserLoginResult.WrongPassword;

            var userType = _context.TypyUzivatelu
                .FromSqlInterpolated($"SELECT * FROM ST67028.TYPY_UZIVATELE WHERE ID_TYP_UZIVATELE = {user.IdTypUzivatele}")
                .FirstOrDefault();

            if (userType == null)
            {
                _logger.Message($"User type of ID: {user.IdTypUzivatele} doesn't exist").Error().Log();
                return UserLoginResult.Failed;
            }

            CreateNewSession(user.UzivatelskeJmeno, userType);
            _logger.Message("User was successfully logged in and Session was updated").Info().Log();
            return UserLoginResult.Success;
        }
        catch (Exception e)
        {
            _logger.Exception(e).Log();
            return UserLoginResult.Failed;
        }
    }

    public void LogoutUser() => App.UserSessionInstance.EndSession();
    
    private string HashPassword(string password)
    {
        var salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password, 
            salt, 
            NumberOfIterations,
            HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32); // 32-byte hash for SHA-256

        //Zkombinovani hashe a soli 
        var hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);

        return Convert.ToBase64String(hashBytes);
    }

    private bool ValidatePassword(string loginPassword, string userPassword)
    {
        var hashBytes = Convert.FromBase64String(userPassword);
        var salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        //Hash nove hesla za pouziti stejne soli a poctu iteraci
        using var pbkdf2 = new Rfc2898DeriveBytes(
            loginPassword,
            salt,
            NumberOfIterations,
            HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32); // 32-byte hash for SHA-256
        
        for (var i = 0; i < 32; i++)
        {
            if (hashBytes[i + 16] != hash[i])
                return false;
        }

        return true;
    }

    private void CreateNewSession(string uzivatelskeJmeno, TypyUzivatele typyUzivatele) 
        => App.UserSessionInstance.UpdateSession(uzivatelskeJmeno, typyUzivatele);
}