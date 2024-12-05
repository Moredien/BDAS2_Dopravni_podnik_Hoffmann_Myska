using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class DBObjektyDTO
{
    [ColumnName("NAZEV_OBJEKTU")]
    public string NazevObjektu { get; set; }
    [ColumnName("TYP_OBJEKTU")]
    public string TypObjektu { get; set; }
}