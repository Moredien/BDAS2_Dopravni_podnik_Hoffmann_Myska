using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class FotoDTO
{
    [ColumnName("ID_FOTO")]
    public int IdFoto{ get; set; }
    [ColumnName("JMENO_SOUBORU")]
    public string JmenoSouboru{ get; set; }
    [ColumnName("DATUM_PRIDANI")]
    public DateTime DatumPridani { get; set; }
    [ColumnName("UZIVATELSKE_JMENO")]
    public string UzivatelskeJmeno{ get; set; }
    [ColumnName("TYP_ASOCIACE")]
    public string TypAsociace { get; set; }
}