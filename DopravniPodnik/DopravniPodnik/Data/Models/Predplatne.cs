using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Predplatne
{
    public decimal IdPredplatneho { get; set; }

    public DateTime Od { get; set; }

    public DateTime Do { get; set; }

    public decimal IdKarty { get; set; }

    public decimal IdTypPredplatneho { get; set; }

    public virtual KartyMhd IdKartyNavigation { get; set; } = null!;

    public virtual TypyPredplatneho IdTypPredplatnehoNavigation { get; set; } = null!;
}
