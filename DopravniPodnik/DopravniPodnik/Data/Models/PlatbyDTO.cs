using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class PlatbyDTO
{
    [ColumnName("CAS_PLATBY")]
    public DateTime CasPlatby { get; set; }
    [ColumnName("VYSE_PLATBY")]
    public double VysePlatby{ get; set; }
    [ColumnName("TYP_PLATBY")]
    public int TypPlatby{ get; set; }
    [ColumnName("TYP_PLATBY_TEXT")]
    public string TypPlatbyText{ get; set; }
    [ColumnName("ID_ZAKAZNIKA")]
    public int IdZakaznika{ get; set; }
    [ColumnName("JMENO_ZAKAZNIKA")]
    public string JmenoZakaznika{ get; set; }
    [ColumnName("DETAIL_PLATBY")]
    public string DetailPlatby{ get; set; }
    [ColumnName("ID_PLATBY")]
    public int IdPlatby{ get; set; }
}