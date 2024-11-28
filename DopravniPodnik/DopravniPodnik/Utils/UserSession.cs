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
    }

    public void EndSession()
    {
        SessionCreationTime = null;
        UserType = null;
        UserName = null;
    }

    public void Temp()
    {
        Console.WriteLine("Test user: " + UserName);
    }
}