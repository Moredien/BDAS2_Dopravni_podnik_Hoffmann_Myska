using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Jizdy
{
    public decimal IdJizdy { get; set; }

    public DateTime Zacatek { get; set; }

    public DateTime? Konec { get; set; }

    public decimal IdVozidla { get; set; }

    public decimal IdLinky { get; set; }

    public virtual Linky IdLinkyNavigation { get; set; } = null!;

    public virtual Vozidla IdVozidlaNavigation { get; set; } = null!;

    public virtual ICollection<Ridici> IdRidices { get; set; } = new List<Ridici>();
}
