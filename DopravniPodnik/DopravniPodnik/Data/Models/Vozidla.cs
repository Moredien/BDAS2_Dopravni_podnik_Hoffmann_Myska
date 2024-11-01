using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Vozidla
{
    public decimal IdVozidla { get; set; }

    public decimal IdTypVozidla { get; set; }

    public virtual TypyVozidel IdTypVozidlaNavigation { get; set; } = null!;

    public virtual ICollection<Jizdy> Jizdies { get; set; } = new List<Jizdy>();
}
