using System;
using DopravniPodnik.Data.Models;

namespace DopravniPodnik.Utils;

public class UserSession
{
    private static UserSession? _instance;
    private static readonly object LockObject = new();

    public string? UserName { get; private set; } = null;
    public TypyUzivatele? UserType { get; private set; } = null;
    public DateTime? SessionCreationTime { get; private set; } = null;
    public bool IsSafeModeOn { get; set; } = false;
    
    private bool _isUserAdmin = false;

    private UserSession() { }

    public static UserSession Instance
    {
        get
        {
            lock (LockObject)
            {
                return _instance ??= new UserSession();
            }
        }
    }

    public void UpdateSession(string? userName, TypyUzivatele? userType)
    {
        UserName = userName;
        UserType = userType;
        SessionCreationTime = DateTime.Now;

        _isUserAdmin = userType is { Nazev: "Admin" };
    }

    public void EndSession()
    {
        SessionCreationTime = null;
        UserType = null;
        UserName = null;
        _isUserAdmin = false;
    }

    public void EmulateUserType(string userType)
    {
        if(!_isUserAdmin) return;
        
        UserType = userType switch
        {
            "Admin" => new TypyUzivatele { Nazev = "Admin" },
            "Zákazník" => new TypyUzivatele { Nazev = "Zákazník" },
            "Zaměstnanec" => new TypyUzivatele { Nazev = "Zaměstnanec" },
            _ => throw new ArgumentException($"Neplatný typ uživatele: {userType}")
        };
    }
}