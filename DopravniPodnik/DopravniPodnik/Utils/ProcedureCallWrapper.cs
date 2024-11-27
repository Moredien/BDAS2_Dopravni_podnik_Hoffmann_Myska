using Oracle.ManagedDataAccess.Client;

namespace DopravniPodnik.Utils;

public class ProcedureCallWrapper(string query, List<OracleParameter> parameters)
{
    public string Query { get; } = query;

    public List<OracleParameter> Parameters { get; } = parameters;
}