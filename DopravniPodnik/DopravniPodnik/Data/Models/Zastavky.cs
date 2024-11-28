using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Zastavky
{    
    [IdProperty]
    [ColumnName("ID_ZASTAVKY")]
    public int IdZastavky { get; set; }

    public string Jmeno { get; set; } = null!;

    public virtual ICollection<Zastaveni> Zastavenis { get; set; } = new List<Zastaveni>();
}
