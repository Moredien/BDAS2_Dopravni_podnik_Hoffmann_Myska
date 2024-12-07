using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class VozovyParkDTO
{
    [IdProperty]
    [ColumnName("ID_VOZIDLA")]
    public int IdVozidla { get; set; }
    [ColumnName("TYP_VOZIDLA")]
    public string TypVozidla { get; set; }
    [ColumnName("ZNACKA")]
    public string Znacka { get; set; }
    [ColumnName("JEZDI_NA_LINCE")]
    public int JezdiNaLince { get; set; }
}