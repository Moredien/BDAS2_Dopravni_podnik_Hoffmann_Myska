using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Zamestnanci
{
    public decimal IdZamestnance { get; set; }

    public int? Plat { get; set; }

    public DateTime? PlatnostUvazkuDo { get; set; }

    public decimal ZamestnanecIdZamestnance { get; set; }

    public decimal UzivatelIdUzivatele { get; set; }

    public virtual ICollection<Zamestnanci> InverseZamestnanecIdZamestnanceNavigation { get; set; } = new List<Zamestnanci>();

    public virtual Uzivatele UzivatelIdUzivateleNavigation { get; set; } = null!;

    public virtual Zamestnanci ZamestnanecIdZamestnanceNavigation { get; set; } = null!;
}
