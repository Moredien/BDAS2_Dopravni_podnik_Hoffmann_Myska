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
        { typeof(UzivatelDTO), "UZIVATEL_VIEW" },
        { typeof(TypyUzivatele), "Typy_uzivatele" },
        { typeof(TypyVozidel), "Typy_vozidel" },
        { typeof(TypyPredplatneho), "Typy_predplatneho" }
    };
}