using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Vozidla
{
    public decimal IdVozidla { get; set; }

    public decimal TypVozidlaIdTypVozidla { get; set; }

    public virtual ICollection<Jizdy> Jizdies { get; set; } = new List<Jizdy>();

    public virtual TypyVozidel TypVozidlaIdTypVozidlaNavigation { get; set; } = null!;
}
