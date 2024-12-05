using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class TypyPredplatneho
{
    [IdProperty]
    [ColumnName("ID_TYP_PREDPLATNEHO")]
    public int IdTypPredplatneho { get; set; }

    public string Jmeno { get; set; } = null!;

    public int Cena { get; set; }

    public virtual ICollection<Predplatne> Predplatnes { get; set; } = new List<Predplatne>();
}
