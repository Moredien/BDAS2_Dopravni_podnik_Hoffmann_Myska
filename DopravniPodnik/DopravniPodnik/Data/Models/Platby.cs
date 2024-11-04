using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Platby
{
    public int IdPlatby { get; set; }

    public DateTime CasPlatby { get; set; }

    public decimal VysePlatby { get; set; }

    public int IdZakaznika { get; set; }

    public virtual Zakaznici IdZakaznikaNavigation { get; set; } = null!;
}
