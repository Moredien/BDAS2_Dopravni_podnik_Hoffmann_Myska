using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class ZastavkyLinkyViewDTO
{
    [IdProperty]
    [ColumnName("ID_LINKY")]
    public int IdLinky { get; set; }
    [ColumnName("CISLO_LINKY")]
    public int CisloLinky{ get; set; }
    [ColumnName("ID_ZASTAVKY")]
    public int IdZastavky{ get; set; }
    [ColumnName("JMENO_ZASTAVKY")]
    public string JmenoZastavky{ get; set; }
    [ColumnName("ODJEZD")]
    public DateTime Odjezd{ get; set; }
    [ColumnName("ITERACE")]
    public int Iterace{ get; set; }
    [ColumnName("SMER")]
    public int Smer{ get; set; }
    [ColumnName("ID_ZASTAVENI")]
    public int IdZastaveni{ get; set; }
}