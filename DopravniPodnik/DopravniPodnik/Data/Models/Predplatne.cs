using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Predplatne
{
    [IdProperty]
    [ColumnName("ID_PREDPLATNEHO")]
    public int IdPredplatneho { get; set; }

    public DateTime Od { get; set; }

    public DateTime Do { get; set; }

    public int IdKarty { get; set; }

    public int IdTypPredplatneho { get; set; }

    public virtual KartyMhd IdKartyNavigation { get; set; } = null!;

    public virtual TypyPredplatneho IdTypPredplatnehoNavigation { get; set; } = null!;
}
