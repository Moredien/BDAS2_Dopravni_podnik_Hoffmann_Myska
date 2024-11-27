using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Utils;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace DopravniPodnik.Data.service;

public class AuthService
{
    private readonly OracleDbContext _context = OracleDbContext.Instance;
    private readonly DatabaseService _databaseService = new();
    private readonly Logger _logger = App.LoggerInstance;

    private const int NumberOfIterations = 10000;

    private const string InsertUserProcedure = """
                                             BEGIN
                                                 InsertUzivatel(
                                                     :p_uzivatelske_jmeno, :p_heslo, :p_jmeno, :p_prijmeni,
                                                     :p_cas_zalozeni, :p_datum_narozeni, :p_typ_uzivatele_nazev,
                                                     :p_mesto, :p_ulice, :p_cislo_popisne, :p_foto_jmeno_souboru,
                                                     :p_foto_data, :p_foto_datum_pridani, :p_error_message
                                                 );
                                             END;
                                             """;
    public UserRegistrationResult RegisterUser(UzivatelDTO uzivatel)
    {
        try
        {
            var existingUser = CheckUserExist(uzivatel.uzivatelske_jmeno);
            if (existingUser)
            {
                _logger.Message($"User with uzivatelske jmeno {uzivatel.uzivatelske_jmeno} already exists").Warning()
                    .Log();
                return UserRegistrationResult.AlreadyRegistered;
            }

            var hashedPassword = HashPassword(uzivatel.heslo);

            var parameters = new []
            {
                new OracleParameter("p_uzivatelske_jmeno", OracleDbType.Varchar2, uzivatel.uzivatelske_jmeno, ParameterDirection.Input),
                new OracleParameter("p_heslo", OracleDbType.Varchar2, hashedPassword, ParameterDirection.Input),
                new OracleParameter("p_jmeno", OracleDbType.Varchar2, uzivatel.jmeno, ParameterDirection.Input),
                new OracleParameter("p_prijmeni", OracleDbType.Varchar2, uzivatel.prijmeni, ParameterDirection.Input),
                new OracleParameter("p_cas_zalozeni", OracleDbType.Date, DateTime.Now, ParameterDirection.Input),
                new OracleParameter("p_datum_narozeni", OracleDbType.Date, uzivatel.datum_narozeni, ParameterDirection.Input),
                new OracleParameter("p_typ_uzivatele_nazev", OracleDbType.Varchar2, uzivatel.nazev_typ_uzivatele, ParameterDirection.Input),
                new OracleParameter("p_mesto", OracleDbType.Varchar2, uzivatel.mesto, ParameterDirection.Input),
                new OracleParameter("p_ulice", OracleDbType.Varchar2, uzivatel.ulice, ParameterDirection.Input),
                new OracleParameter("p_cislo_popisne", OracleDbType.Decimal, uzivatel.cislo_popisne, ParameterDirection.Input),
                new OracleParameter("p_foto_jmeno_souboru", OracleDbType.Varchar2,
                    string.IsNullOrEmpty(uzivatel.foto_jmeno_souboru) ? DBNull.Value : uzivatel.foto_jmeno_souboru, ParameterDirection.Input),
                new OracleParameter("p_foto_data", OracleDbType.Blob,
                    uzivatel.foto_data != null ? ImageToBlob(uzivatel.foto_data) : DBNull.Value, ParameterDirection.Input),
                new OracleParameter("p_foto_datum_pridani", OracleDbType.Date,
                    uzivatel.foto_datum_pridani != default ? uzivatel.foto_datum_pridani : DBNull.Value, ParameterDirection.Input),
                new OracleParameter("p_error_message", OracleDbType.Varchar2, 4000) { Direction = ParameterDirection.Output }
            };

            var success = _databaseService.CallDbProcedure(InsertUserProcedure, parameters, out var errorMessage);
            
            if (!success)
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _logger.Message($"Database error: {errorMessage}").Error().Log();
                    return UserRegistrationResult.Failed;
                }

                _logger.Message("User registration failed at the database level").Error().Log();
                return UserRegistrationResult.Failed;
            }

            _logger.Message("User was successfully inserted into DB").Info().Log();

            var userTypes = _databaseService.FetchData<TypyUzivatele>("SELECT * FROM ST67028.TYPY_UZIVATELE");
            CreateNewSession(uzivatel.uzivatelske_jmeno, userTypes.First(e => e.Nazev == uzivatel.nazev_typ_uzivatele));

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
                .OrderBy(u => u.UzivatelskeJmeno)
                .FirstOrDefault();
            
            if (user == null)
            {
                _logger.Message($"User with name: {uzivatelskeJmeno} doesn't exist").Warning().Log();
                return UserLoginResult.NotRegistered;
            }

            if (!ValidatePassword(heslo, user.Heslo)) return UserLoginResult.WrongPassword;

            var userType = _context.TypyUzivatelu
                .FromSqlInterpolated($"SELECT * FROM ST67028.TYPY_UZIVATELE WHERE ID_TYP_UZIVATELE = {user.IdTypUzivatele}")
                .OrderBy(t => t.IdTypUzivatele)
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

    private bool CheckUserExist(string username)
    {
        //Kvuli tomu ze GetDbConnection obecny DbContext, tak to musi castnout na OracleConnection, abych tu connection mohl pouzit v OracleCommand
        var connection = _context.Database.GetDbConnection() as OracleConnection;
        if (connection == null)
        {
            _logger.Message("GetDbConnection wasn't of type OracleConnection").Error().Log();
            return false;
        }

        try
        {
            connection.Open();

            using var command = new OracleCommand("CheckUserExists", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;

            var outputParam = new OracleParameter("p_exists", OracleDbType.Int32)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            command.Parameters.Add(outputParam);

            command.ExecuteNonQuery();

            return ((OracleDecimal)outputParam.Value).ToInt32() == 1;
        }
        catch (Exception e)
        {
            _logger.Exception(e).Log();
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    private byte[]? ImageToBlob(Image? image)
    {
        if (image == null) return null;

        using var memoryStream = new MemoryStream();
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)image.Source));
        encoder.Save(memoryStream);
        return memoryStream.ToArray();
    }


    private void CreateNewSession(string uzivatelskeJmeno, TypyUzivatele typyUzivatele)
        => App.UserSessionInstance.UpdateSession(uzivatelskeJmeno, typyUzivatele);
}