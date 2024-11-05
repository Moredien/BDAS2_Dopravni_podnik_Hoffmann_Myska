using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zakaznici
{
    public int IdZakaznika { get; set; }

    public int IdUzivatele { get; set; }

    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual ICollection<KartyMhd> KartyMhds { get; set; } = new List<KartyMhd>();

    public virtual ICollection<Platby> Platbies { get; set; } = new List<Platby>();
}
