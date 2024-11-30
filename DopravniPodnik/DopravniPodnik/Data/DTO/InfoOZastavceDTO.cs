using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class InfoOZastavceDTO
{
    [ColumnName("LINKA")]
    public int Linka { get; set; }
    [ColumnName("JMENO_LINKY")]
    public string JmenoLinky { get; set; }
    [ColumnName("CAS_ODJEZDU")]
    public DateTime Odjezd { get; set; }
}