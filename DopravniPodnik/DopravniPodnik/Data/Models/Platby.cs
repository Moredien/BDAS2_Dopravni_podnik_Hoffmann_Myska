using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Platby
{
    public decimal IdPlatby { get; set; }

    public DateTime CasPlatby { get; set; }

    public decimal VysePlatby { get; set; }

    public decimal ZakaznikIdZakaznika { get; set; }

    public virtual Zakaznici ZakaznikIdZakaznikaNavigation { get; set; } = null!;
}
