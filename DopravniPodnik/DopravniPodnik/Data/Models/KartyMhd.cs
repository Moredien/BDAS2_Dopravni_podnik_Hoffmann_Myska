using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class KartyMhd
{
    public int IdKarty { get; set; }

    public decimal Zustatek { get; set; }

    public DateTime PlatnostOd { get; set; }

    public DateTime PlatnostDo { get; set; }

    public int IdZakaznika { get; set; }

    public int IdFoto { get; set; }

    public virtual Foto? Foto { get; set; }

    public virtual Foto IdFotoNavigation { get; set; } = null!;

    public virtual Zakaznici IdZakaznikaNavigation { get; set; } = null!;

    public virtual ICollection<Predplatne> Predplatnes { get; set; } = new List<Predplatne>();
}
