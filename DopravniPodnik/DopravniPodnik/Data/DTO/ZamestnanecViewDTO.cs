using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class ZamestnanecViewDTO
{
    [IdProperty]
    [ColumnName("ID_ZAMESTNANCE")] 
    public int? IdZamestnance { get; set; }
    
    [ColumnName("UZIVATELSKE_JMENO")] 
    public string UzivatelskeJmeno { get; set; }
    
    [ColumnName("JMENO")] 
    public string Jmeno { get; set; }

    [ColumnName("PRIJMENI")] 
    public string Prijmeni { get; set; }
    
    [ColumnName("PLAT")] 
    public int Plat { get; set; }

    [ColumnName("PLATNOST_UVAZKU_DO")] 
    public DateTime? PlatnostUvazkuDo { get; set; }

    [ColumnName("ID_NADRIZENEHO")]
    public int? IdNadrizeneho { get; set; }
    
    [ColumnName("JMENO_NADRIZENEHO")] 
    public string? JmenoNadrizeneho { get; set; }

    [ColumnName("PRIJMENI_NADRIZENEHO")] 
    public string? PrijmeniNadrizeneho { get; set; }
    
    [ColumnName("ID_UZIVATELE")]
    public int? IdUzivatele { get; set; }
}