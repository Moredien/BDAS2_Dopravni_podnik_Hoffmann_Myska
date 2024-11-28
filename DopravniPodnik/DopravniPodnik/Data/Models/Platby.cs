using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Platby
{
    [IdProperty]
    [ColumnName("ID_PLATBY")]
    public int IdPlatby { get; set; }

    public DateTime CasPlatby { get; set; }

    public decimal VysePlatby { get; set; }

    public int IdZakaznika { get; set; }

    public virtual Zakaznici IdZakaznikaNavigation { get; set; } = null!;
}
