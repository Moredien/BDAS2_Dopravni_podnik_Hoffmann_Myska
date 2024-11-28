using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Linky
{
    [IdProperty]
    [ColumnName("ID_LINKY")]
    public int IdLinky { get; set; }

    public short CisloLinky { get; set; }

    public string? Jmeno { get; set; }

    public virtual ICollection<Jizdy> Jizdies { get; set; } = new List<Jizdy>();

    public virtual ICollection<Zastaveni> Zastavenis { get; set; } = new List<Zastaveni>();
}
