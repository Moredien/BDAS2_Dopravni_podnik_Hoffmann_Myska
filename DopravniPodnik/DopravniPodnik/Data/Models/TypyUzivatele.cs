using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class TypyUzivatele
{
    public decimal IdTypUzivatele { get; set; }

    public string Nazev { get; set; } = null!;

    public virtual ICollection<Uzivatele> Uzivateles { get; set; } = new List<Uzivatele>();
}
