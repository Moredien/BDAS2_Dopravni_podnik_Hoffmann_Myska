using System.Text;
using DopravniPodnik.Data.Models;
using DopravniPodnik.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DopravniPodnik.Data.service;

public class AuthService
{
    private readonly OracleDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    private UserSession? _userSession;
    
    
    private AuthService(OracleDbContext context, ServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
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
        if (_userSession == null) return;
        
        Console.WriteLine("User logged out. Session destroyed.");
        _userSession = null;
    }
    
    //TODO udelat poradny hash
    private string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        return Convert.ToBase64String(bytes);
    }

    private bool ValidatePassword(string loginPassword, string userPassword)
    {
        return userPassword == HashPassword(loginPassword);
    }

    private bool CreateNewSession(string uzivatelskeJmeno, TypyUzivatele typyUzivatele)
    {
        _userSession = _serviceProvider.GetService<UserSession>();
        if(_userSession == null) return false;
        
        _userSession.UserName = uzivatelskeJmeno;
        _userSession.UserType = typyUzivatele;
        
        return true;
    }
}