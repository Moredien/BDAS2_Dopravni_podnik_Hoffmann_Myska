using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zamestnanci
{
    public int IdZamestnance { get; set; }

    public string? Plat { get; set; }

    public DateTime? PlatnostUvazkuDo { get; set; }

    public int IdNadrizeneho { get; set; }

    public int IdUzivatele { get; set; }

    public virtual Zamestnanci IdNadrizenehoNavigation { get; set; } = null!;

    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual ICollection<Zamestnanci> InverseIdNadrizenehoNavigation { get; set; } = new List<Zamestnanci>();
}
