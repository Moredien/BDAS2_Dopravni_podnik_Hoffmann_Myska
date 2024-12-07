using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class JizdyViewDTO
{
    [IdProperty]
    [ColumnName("ID_JIZDY")]
    public int IdJizdy{ get; set; }
    [ColumnName("ID_LINKY")]
    public int IdLinky{ get; set; }
    [ColumnName("ID_VOZIDLA")]
    public int IdVozidla{ get; set; }
    [ColumnName("ID_RIDICE")]
    public int IdRidice{ get; set; }
    [ColumnName("ZACATEK")]
    public DateTime Zacatek{ get; set; }
    [ColumnName("KONEC")]
    public DateTime? Konec{ get; set; }
    [ColumnName("CISLO_LINKY")]
    public int CisloLinky{ get; set; }
    [ColumnName("JMENO_LINKY")]
    public string? JmenoLinky{ get; set; }
    [ColumnName("ZNACKA_VOZIDLA")]
    public string? ZnackaVozidla{ get; set; }
    [ColumnName("TYP_VOZIDLA")]
    public string TypVozidla{ get; set; }
    [ColumnName("JMENO_RIDICE")]
    public string JmenoRidice{ get; set; }
    [ColumnName("PRIJMENI_RIDICE")]
    public string PrijmeniRidice{ get; set; }
}