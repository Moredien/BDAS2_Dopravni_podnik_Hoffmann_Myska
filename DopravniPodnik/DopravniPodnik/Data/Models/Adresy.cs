using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Adresy
{
    public decimal IdAdresy { get; set; }

    public string Mesto { get; set; } = null!;

    public string Ulice { get; set; } = null!;

    public short CisloPopisne { get; set; }

    public virtual ICollection<Uzivatele> Uzivateles { get; set; } = new List<Uzivatele>();
}
