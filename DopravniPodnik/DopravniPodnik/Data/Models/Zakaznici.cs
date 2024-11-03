using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zakaznici
{
    public decimal IdZakaznika { get; set; }

    public decimal IdUzivatele { get; set; }

    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual ICollection<KartyMhd> KartyMhds { get; set; } = new List<KartyMhd>();

    public virtual ICollection<Platby> Platbies { get; set; } = new List<Platby>();
}
