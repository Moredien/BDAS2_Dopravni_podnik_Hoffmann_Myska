﻿using System.Windows.Controls;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class UzivatelDTO
{
    [IdProperty]
    public int? id_uzivatele { get; set; }
    public string uzivatelske_jmeno{ get; set; }
    public string heslo{ get; set; }
    public string jmeno{ get; set; }
    public string prijmeni{ get; set; }
    public DateTime cas_zalozeni{ get; set; }
    public DateTime datum_narozeni{ get; set; }
    //typ_uzivatele
    [ColumnName("TYP_UZIVATELE")]
    public string nazev_typ_uzivatele{ get; set; }
    //foto
    public int? id_foto{ get; set; }
    //adresa
    public int id_adresy{ get; set; }
    public string mesto{ get; set; }
    public string ulice{ get; set; }
    public short cislo_popisne{ get; set; }
    //possible relations
    public int? id_zakaznika{ get; set; }
    public int? id_zamestnance{ get; set; }
    
}