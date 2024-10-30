using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Jizdy
{
    public decimal IdJizdy { get; set; }

    public DateTime Zacatek { get; set; }

    public DateTime? Konec { get; set; }

    public decimal VozidloIdVozidla { get; set; }

    public decimal LinkaIdLinky { get; set; }

    public virtual Linky LinkaIdLinkyNavigation { get; set; } = null!;

    public virtual Vozidla VozidloIdVozidlaNavigation { get; set; } = null!;

    public virtual ICollection<Ridici> RidicIdRidices { get; set; } = new List<Ridici>();
}
