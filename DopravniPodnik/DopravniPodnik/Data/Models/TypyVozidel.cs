using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class TypyVozidel
{
    public int IdTypVozidla { get; set; }

    public string Nazev { get; set; } = null!;

    public string Znacka { get; set; } = null!;

    public virtual ICollection<Vozidla> Vozidlas { get; set; } = new List<Vozidla>();
}
