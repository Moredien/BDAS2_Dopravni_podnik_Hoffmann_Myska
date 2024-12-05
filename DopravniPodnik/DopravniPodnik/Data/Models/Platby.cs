using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Platby
{
    [IdProperty]
    [ColumnName("ID_PLATBY")]
    public int IdPlatby { get; set; }
    [ColumnName("CAS_PLATBY")]
    public DateTime CasPlatby { get; set; }
    [ColumnName("VYSE_PLATBY")]
    public decimal VysePlatby { get; set; }
    [ColumnName("ID_ZAKAZNI")]
    public int IdZakaznika { get; set; }
    
    [ColumnName("TYP_PLATBY")]
    public int TypPlatby { get; set; }
    public virtual Zakaznici IdZakaznikaNavigation { get; set; } = null!;
    
    public string TypPlatbyString => TypPlatby switch
    {
        0 => "Kartou",
        1 => "Převodem",
        _ => ""
    };
}
