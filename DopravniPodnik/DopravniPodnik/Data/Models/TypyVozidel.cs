using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class TypyVozidel
{
    [IdProperty]
    [ColumnName("ID_TYP_VOZIDLA")]
    public int IdTypVozidla { get; set; }

    public string Nazev { get; set; } = null!;

    public string Znacka { get; set; } = null!;

    public virtual ICollection<Vozidla> Vozidlas { get; set; } = new List<Vozidla>();
}
