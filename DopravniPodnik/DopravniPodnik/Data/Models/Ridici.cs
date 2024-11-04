using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Ridici
{
    public int IdRidice { get; set; }

    public string Jmeno { get; set; } = null!;

    public string Prijmeni { get; set; } = null!;

    public virtual ICollection<Jizdy> IdJizdies { get; set; } = new List<Jizdy>();
}
