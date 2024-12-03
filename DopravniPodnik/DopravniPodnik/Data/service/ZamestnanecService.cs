using System.Data;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Utils;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.Data.service;

public class ZamestnanecService
{
    private readonly DatabaseService _databaseService = new();
    private readonly Logger _logger = Logger.Instance;
    
    private const string GetZamestnanceQuery = "SELECT * FROM ST67028.ZAMESTNANCI_VIEW";
    private const string PovysitZamestnaceProcedure = @"
            BEGIN
                ST67028.POVYSIT_ZAMESTNANCE(
                    :p_id_zamestnance,
                    :p_novy_plat,
                    :p_id_noveho_nadrizeneho,
                    :p_id_podrizenych
                );
            END;
        ";

    public List<ZamestnanecViewDTO> GetAllZamestnanci()
    {
        try
        {
            return _databaseService.FetchData<ZamestnanecViewDTO>(GetZamestnanceQuery);
        }
        catch (Exception ex)
        {
            _logger.Message($"Chyba při načítání zaměstnanců: {ex.Message}").Error().Log();
            return [];
        }
    }
    
    /// <summary>
    /// Volá proceduru POVYSIT_ZAMESTNANCE pro povýšení zaměstnance.
    /// </summary>
    /// <param name="idZamestnance">ID zaměstnance, který má být povýšen.</param>
    /// <param name="novyPlat">Nový plat povýšeného zaměstnance.</param>
    /// <param name="idNovehoNadrizeneho">ID nového nadřízeného (volitelný parametr).</param>
    /// <param name="podrizeni">Seznam zaměstnanců, kteří budou podřízeni (volitelný parametr).</param>
    /// <returns>Vrací true, pokud proběhlo úspěšně, jinak false.</returns>
    public bool PovysitZamestnance(
        int idZamestnance,
        decimal novyPlat,
        int? idNovehoNadrizeneho = null,
        List<ZamestnanecViewDTO>? podrizeni = null)
    {
        try
        {
            var parameters = new List<OracleParameter>
            {
                new("p_id_zamestnance", OracleDbType.Int32, idZamestnance, ParameterDirection.Input),
                new("p_novy_plat", OracleDbType.Decimal, novyPlat, ParameterDirection.Input),
                new("p_id_noveho_nadrizeneho", OracleDbType.Int32, idNovehoNadrizeneho ?? (object)DBNull.Value, ParameterDirection.Input),
                new("p_id_podrizenych", OracleDbType.Varchar2, 
                    podrizeni != null ? ConvertPodrizeniToString(podrizeni) : DBNull.Value, 
                    ParameterDirection.Input)
            };

            var procedureCallWrapper = new ProcedureCallWrapper(PovysitZamestnaceProcedure, parameters);

            _databaseService.ExecuteDbCall(procedureCallWrapper, out var error);

            if (string.IsNullOrEmpty(error)) return true;
            
            _logger.Message($"Chyba při povyšování zaměstnance: {error}").Error().Log();
            return false;

        }
        catch (Exception ex)
        { 
            _logger.Exception(ex).Log();
            return false;
        }
    }
    
    private string ConvertPodrizeniToString(List<ZamestnanecViewDTO> podrizeni)
    {
        return string.Join(",", podrizeni.Select(z => z.IdZamestnance));
    }
}