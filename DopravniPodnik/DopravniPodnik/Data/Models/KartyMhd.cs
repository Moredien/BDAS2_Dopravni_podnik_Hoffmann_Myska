using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class KartyMhd
{
    public decimal IdKarty { get; set; }

    public decimal Zustatek { get; set; }

    public DateTime PlatnostOd { get; set; }

    public DateTime PlatnostDo { get; set; }

    public decimal IdZakaznika { get; set; }

    public decimal IdFoto { get; set; }

    public virtual Foto? Foto { get; set; }
    
    public virtual Zakaznici IdZakaznikaNavigation { get; set; } = null!;

    public virtual ICollection<Predplatne> Predplatnes { get; set; } = new List<Predplatne>();
}
