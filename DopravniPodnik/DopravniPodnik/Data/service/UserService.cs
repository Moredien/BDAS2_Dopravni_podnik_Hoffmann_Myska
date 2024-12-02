using DopravniPodnik.Data.DTO;
using Microsoft.EntityFrameworkCore;

namespace DopravniPodnik.Data.service;

[Obsolete("Use DopravniPodnik.Data.DatabaseService#FetchData instead.")]
public class UserService
{
    private readonly OracleDbContext _context = OracleDbContext.Instance;
    
    private string FetchAllUsersSql => @"
    SELECT 
        U.UZIVATELSKE_JMENO,
        U.HESLO,
        U.JMENO,
        U.PRIJMENI,
        U.CAS_ZALOZENI,
        U.DATUM_NAROZENI,
        TU.NAZEV AS NAZEV_TYP_UZIVATELE,
        A.ID_ADRESY,
        A.MESTO,
        A.ULICE,
        A.CISLO_POPISNE
    FROM 
        ST67028.UZIVATELE U
    JOIN 
        ST67028.TYPY_UZIVATELE TU ON U.ID_TYP_UZIVATELE = TU.ID_TYP_UZIVATELE
    JOIN 
        ST67028.ADRESY A ON U.ID_ADRESY = A.ID_ADRESY";
    
    
    [Obsolete]
    public List<UzivatelDTO> FetchAllUsers() 
    {
        var uzivateleDtos = new List<UzivatelDTO>();
    
        // Get the database connection
        var connection = _context.Database.GetDbConnection();
        connection.Open();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = FetchAllUsersSql;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var uzivatelDto = new UzivatelDTO
                    {
                        uzivatelske_jmeno = reader.GetString(reader.GetOrdinal("UZIVATELSKE_JMENO")),
                        heslo = reader.GetString(reader.GetOrdinal("HESLO")),
                        jmeno = reader.GetString(reader.GetOrdinal("JMENO")),
                        prijmeni = reader.GetString(reader.GetOrdinal("PRIJMENI")),
                        cas_zalozeni = reader.GetDateTime(reader.GetOrdinal("CAS_ZALOZENI")),
                        datum_narozeni = reader.GetDateTime(reader.GetOrdinal("DATUM_NAROZENI")),
                        nazev_typ_uzivatele = reader.GetString(reader.GetOrdinal("NAZEV_TYP_UZIVATELE")),
                        id_adresy = reader.GetInt32(reader.GetOrdinal("ID_ADRESY")),
                        mesto = reader.GetString(reader.GetOrdinal("MESTO")),
                        ulice = reader.GetString(reader.GetOrdinal("ULICE")),
                        cislo_popisne = reader.GetInt16(reader.GetOrdinal("CISLO_POPISNE")),
                        // Optional zatim default hodnoty
                        id_foto = 0,
                        foto_jmeno_souboru = string.Empty,
                        foto_data = null,
                        foto_datum_pridani = DateTime.MinValue,
                        id_zakaznika = null,
                        id_zamestnance = null
                };

                uzivateleDtos.Add(uzivatelDto);
                } 
            }
        }

        connection.Close();
        return uzivateleDtos;
    }
}