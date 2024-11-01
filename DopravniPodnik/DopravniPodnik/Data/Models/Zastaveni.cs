using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zastaveni
{
    public decimal IdZastaveni { get; set; }

    public DateTime Odjezd { get; set; }

    public decimal IdLinky { get; set; }

    public decimal IdZastavky { get; set; }

    public virtual Linky IdLinkyNavigation { get; set; } = null!;

    public virtual Zastavky IdZastavkyNavigation { get; set; } = null!;
}
