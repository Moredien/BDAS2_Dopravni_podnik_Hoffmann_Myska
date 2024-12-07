using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class KartyMhdViewDTO
{
    [ColumnName("ID_KARTY")] 
    public int IdKarty{ get; set; }
    [ColumnName("ZUSTATEK")] 
    public int Zustatek{ get; set; }
    [ColumnName("PLATNOST_OD")] 
    public DateTime PlatnostOd{ get; set; }
    [ColumnName("PLATNOST_DO")] 
    public DateTime PlatnostDo{ get; set; }
    [ColumnName("ID_FOTO")] 
    public int IdFoto{ get; set; }
    [ColumnName("UZIVATELSKE_JMENO")] 
    public string UzivatelskeJmeno{ get; set; }
    [ColumnName("JMENO")] 
    public string Jmeno{ get; set; }
    [ColumnName("PRIJMENI")] 
    public string Prijmeni{ get; set; }
}