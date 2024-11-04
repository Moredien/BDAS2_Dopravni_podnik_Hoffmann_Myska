using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zastaveni
{
    public int IdZastaveni { get; set; }

    public DateTime Odjezd { get; set; }

    public int IdLinky { get; set; }

    public int IdZastavky { get; set; }

    public virtual Linky IdLinkyNavigation { get; set; } = null!;

    public virtual Zastavky IdZastavkyNavigation { get; set; } = null!;
}
