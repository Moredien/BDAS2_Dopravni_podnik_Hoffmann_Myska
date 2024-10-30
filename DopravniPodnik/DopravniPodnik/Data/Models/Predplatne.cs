using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Predplatne
{
    public decimal IdPredplatneho { get; set; }

    public DateTime Od { get; set; }

    public DateTime Do { get; set; }

    public decimal KartaMhdIdKarty { get; set; }

    public decimal TypPIdTypPredplatneho { get; set; }

    public virtual KartyMhd KartaMhdIdKartyNavigation { get; set; } = null!;

    public virtual TypyPredplatneho TypPIdTypPredplatnehoNavigation { get; set; } = null!;
}
