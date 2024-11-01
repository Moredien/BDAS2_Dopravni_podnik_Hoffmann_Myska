using DopravniPodnik.Data.Models;

namespace DopravniPodnik.Utils;

public class UserSession(string userName, TypyUzivatele userType)
{
    public string? UserName { get; set; } = userName;
    public TypyUzivatele? UserType { get; set; } = userType;
    public DateTime? SessionCreationTime { get; } = DateTime.Now;
}