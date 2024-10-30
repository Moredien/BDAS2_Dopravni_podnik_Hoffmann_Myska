using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class TypyPredplatneho
{
    public decimal IdTypPredplatneho { get; set; }

    public string Jmeno { get; set; } = null!;

    public decimal Cena { get; set; }

    public virtual ICollection<Predplatne> Predplatnes { get; set; } = new List<Predplatne>();
}
