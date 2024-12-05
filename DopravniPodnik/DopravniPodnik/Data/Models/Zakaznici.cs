using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Zakaznici
{
    [IdProperty]
    [ColumnName("ID_ZAKAZNIKA")]
    public int IdZakaznika { get; set; }
    [ColumnName("ID_UZIVATELE")]
    public int IdUzivatele { get; set; }

    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual ICollection<KartyMhd> KartyMhds { get; set; } = new List<KartyMhd>();

    public virtual ICollection<Platby> Platbies { get; set; } = new List<Platby>();
}
