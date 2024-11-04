using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zastavky
{
    public int IdZastavky { get; set; }

    public string Jmeno { get; set; } = null!;

    public virtual ICollection<Zastaveni> Zastavenis { get; set; } = new List<Zastaveni>();
}
