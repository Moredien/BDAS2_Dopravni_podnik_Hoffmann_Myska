using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Jizdy
{
    public int IdJizdy { get; set; }

    public DateTime Zacatek { get; set; }

    public DateTime? Konec { get; set; }

    public int IdVozidla { get; set; }

    public int IdLinky { get; set; }

    public virtual Linky IdLinkyNavigation { get; set; } = null!;

    public virtual Vozidla IdVozidlaNavigation { get; set; } = null!;

    public virtual ICollection<Ridici> IdRidices { get; set; } = new List<Ridici>();
}
