using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;

namespace DopravniPodnik.Utils;

public static class TableMapper
{
    public static string getTableName(Type DTO)
    {
        if (tableNames.ContainsKey(DTO))
        {
            return tableNames[DTO];
        }
            return DTO.Name;
    }
//only necessary if the DTO type name is different from the table name 
    private static Dictionary<Type, string> tableNames = new()
    {
        { typeof(UzivatelDTO), "ZakaznikView" },
        { typeof(TypyUzivatele), "Typy_uzivatele" }
    };
}