using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Ridici
{
    [IdProperty]
    [ColumnName("ID_RIDICE")]
    public int IdRidice { get; set; }

    public string Jmeno { get; set; } = null!;

    public string Prijmeni { get; set; } = null!;

    public virtual ICollection<Jizdy> IdJizdies { get; set; } = new List<Jizdy>();
}
