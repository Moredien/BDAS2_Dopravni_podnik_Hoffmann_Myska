using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class KartyMhd
{
    public decimal IdKarty { get; set; }

    public decimal Zustatek { get; set; }

    public DateTime PlatnostOd { get; set; }

    public DateTime PlatnostDo { get; set; }

    public decimal ZakaznikIdZakaznika { get; set; }

    public decimal FotoIdFoto { get; set; }

    public virtual Foto? Foto { get; set; }

    public virtual Foto FotoIdFotoNavigation { get; set; } = null!;

    public virtual ICollection<Predplatne> Predplatnes { get; set; } = new List<Predplatne>();

    public virtual Zakaznici ZakaznikIdZakaznikaNavigation { get; set; } = null!;
}
