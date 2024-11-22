using System.Windows.Controls;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class UzivatelDTO
{
    public string uzivatelske_jmeno{ get; set; }
    public string heslo{ get; set; }
    public string jmeno{ get; set; }
    public string prijmeni{ get; set; }
    public DateTime datum_zalozeni{ get; set; }
    public DateTime datum_narozeni{ get; set; }
    //typ_uzivatele
    [ColumnName("TYP_UZIVATELE")]
    public string nazev_typ_uzivatele{ get; set; }
    //foto
    public int id_foto{ get; set; }
    public string foto_jmeno_souboru{ get; set; }
    public Image? foto_data{ get; set; }
    public DateTime foto_datum_pridani{ get; set; }
    //adresa
    public int id_adresy{ get; set; }
    public string mesto{ get; set; }
    public string ulice{ get; set; }
    public short cislo_popisne{ get; set; }
    //possible relations
    public int? id_zakaznika{ get; set; }
    public int? id_zamestnance{ get; set; }
    
}