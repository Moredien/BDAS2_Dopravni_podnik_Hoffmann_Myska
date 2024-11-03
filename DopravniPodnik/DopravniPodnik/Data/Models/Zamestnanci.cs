using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zamestnanci
{
    public decimal IdZamestnance { get; set; }

    public int? Plat { get; set; }

    public DateTime? PlatnostUvazkuDo { get; set; }

    public decimal IdNadrizeneho { get; set; }

    public decimal IdUzivatele { get; set; }

    public virtual Zamestnanci IdNadrizenehoNavigation { get; set; } = null!;

    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual ICollection<Zamestnanci> InverseIdNadrizenehoNavigation { get; set; } = new List<Zamestnanci>();
}
