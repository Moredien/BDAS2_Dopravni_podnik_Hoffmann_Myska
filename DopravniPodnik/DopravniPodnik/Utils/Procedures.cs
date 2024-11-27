using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.Utils;

public static class Procedures
{
    public static ProcedureCallWrapper GetLinkyForZastavkyProcedure(string startZastavka, string endZastavka)
    {
        var query = """
                        DECLARE
                            result SYS_REFCURSOR;
                        BEGIN
                            ST67028.FIND_LINKA_FOR_STOPS(:startStop, :endStop, result);
                            OPEN :cursor FOR SELECT ID_LINKY, CISLO_LINKY, ID_ZASTAVKY, JMENO_ZASTAVKY, ODJEZD FROM ST67028.ZASTAVKY_LINKY_VIEW;
                        END;
                    """;

        var parameters = new List<OracleParameter>
        {
            new("startStop", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Value = startZastavka
            },
            new("endStop", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Value = endZastavka
            },
            new("cursor", OracleDbType.RefCursor)
            {
                Direction = ParameterDirection.Output
            }
        };
        
        return new ProcedureCallWrapper(query, parameters);
    }
}