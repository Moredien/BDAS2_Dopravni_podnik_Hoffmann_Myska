using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class TypyUzivatele
{
    [IdProperty]
    [ColumnName("ID_TYP_UZIVATELE")]
    public int IdTypUzivatele { get; set; }

    public string Nazev { get; set; } = null!;

    public virtual ICollection<Uzivatele> Uzivateles { get; set; } = new List<Uzivatele>();
}
