using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Adresy
{
    [IdProperty]
    [ColumnName("ID_ADRESY")]
    public int IdAdresy { get; set; }

    public string Mesto { get; set; } = null!;

    public string Ulice { get; set; } = null!;

    [ColumnName("CISLO_POPISNE")]
    public int CisloPopisne { get; set; }

    public virtual ICollection<Uzivatele> Uzivateles { get; set; } = new List<Uzivatele>();
}
