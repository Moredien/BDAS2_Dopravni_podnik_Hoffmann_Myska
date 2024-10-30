using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zastaveni
{
    public decimal IdZastaveni { get; set; }

    public DateTime Odjezd { get; set; }

    public decimal LinkaIdLinky { get; set; }

    public decimal ZastavkaIdZastavky { get; set; }

    public virtual Linky LinkaIdLinkyNavigation { get; set; } = null!;

    public virtual Zastavky ZastavkaIdZastavkyNavigation { get; set; } = null!;
}
