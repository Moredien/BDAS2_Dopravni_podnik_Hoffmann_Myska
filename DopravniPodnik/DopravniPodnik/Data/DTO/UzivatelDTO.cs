using System.Windows.Controls;

namespace DopravniPodnik.Data.DTO;

public class UzivatelDTO
{
    // uzivatel
    // public decimal id_uzivatele { get; set; }
    public string uzivatelske_jmeno{ get; set; }
    public string heslo{ get; set; }
    public string jmeno{ get; set; }
    public string prijmeni{ get; set; }
    public DateTime datum_zalozeni{ get; set; }
    public DateTime datum_narozeni{ get; set; }
    //typ_uzivatele
    // public decimal id_typ_uzivatele{ get; set; }
    public string nazev_typ_uzivatele{ get; set; }
    //foto
    public decimal id_foto{ get; set; }
    public string foto_jmeno_souboru{ get; set; }
    public Image foto_data{ get; set; }
    public DateTime foto_datum_pridani{ get; set; }
    //adresa
    public decimal id_adresy{ get; set; }
    public string mesto{ get; set; }
    public string ulice{ get; set; }
    public short cislo_popisne{ get; set; }
    //possible relations
    public decimal? id_zakaznika{ get; set; }
    public decimal? id_zamestnance{ get; set; }
    
}