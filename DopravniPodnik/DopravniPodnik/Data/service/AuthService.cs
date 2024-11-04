using System.Security.Cryptography;
using System.Text;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DopravniPodnik.Data.service;

public class AuthService
{
    private readonly OracleDbContext _context;
    
    private const int NumberOfIterations = 10000;
    
    private AuthService()
    {
        _context = OracleDbContext.Instance;
    }

    public async Task<UserRegistrationResult> RegisterUser(Uzivatele uzivatel)
    {
        try
        {
            Uzivatele? existingUser = await _context.Uzivatele
                .FromSqlRaw("SELECT * FROM Uzivatele WHERE UzivatelskeJmeno = {0}", uzivatel.UzivatelskeJmeno)
                .FirstOrDefaultAsync();

            if (existingUser != null) return UserRegistrationResult.AlreadyRegistered;

            var hashedPassword = HashPassword(uzivatel.Heslo);
            var casZalozeni = DateTime.Now;

            //TODO domluvit se co vsechno bude potreba pro registraci
            var result = await _context.Database
                .ExecuteSqlRawAsync(
                    "INSERT INTO Uzivatele (UzivatelskeJmeno, Heslo, Jmeno, Prijmeni, CasZalozeni, DatumNarozeni ) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    uzivatel.UzivatelskeJmeno,
                    hashedPassword,
                    uzivatel.Prijmeni,
                    casZalozeni,
                    uzivatel.DatumNarozeni);

            return result == 1 ? UserRegistrationResult.Success : UserRegistrationResult.Failed;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return UserRegistrationResult.Failed;
        }
    }

    public async Task<UserLoginResult> LoginUser(string uzivatelskeJmeno, string heslo)
    {
        try
        {
            var user = await _context.Uzivatele
                .FromSqlRaw("SELECT * FROM WHERE UzivatelskeJmeno = {0}", uzivatelskeJmeno)
                .FirstOrDefaultAsync();

            if (user == null ) return UserLoginResult.Failed;
            
            if(!ValidatePassword(heslo, user.Heslo)) return UserLoginResult.WrongPassword;

            var userType = await _context.TypyUzivatelu.FromSqlRaw(
                    "SELECT * FROM TypyUzivatele WHERE IdTypUzivatele = {0}", user.IdTypUzivatele)
                .FirstOrDefaultAsync();
            
            if(userType == null) return UserLoginResult.Failed;
            
            return !CreateNewSession(user.UzivatelskeJmeno, userType) ? UserLoginResult.Failed : UserLoginResult.Success;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return UserLoginResult.Failed;
        }
    }
    
    public void LogoutUser()
    {
        var userSession = UserSession.Instance;
        userSession.EndSession();
    }
    
    //TODO udelat poradny hash
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

    private bool CreateNewSession(string uzivatelskeJmeno, TypyUzivatele typyUzivatele)
    {
        var userSession = UserSession.Instance;
        
        userSession.UpdateSession(uzivatelskeJmeno, typyUzivatele);
        return true;
    }
}