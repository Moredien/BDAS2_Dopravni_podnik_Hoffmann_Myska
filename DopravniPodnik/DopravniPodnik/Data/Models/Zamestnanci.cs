using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Zamestnanci
{
    public int IdZamestnance { get; set; }

    public string? Plat { get; set; }

    [ColumnName("PLATNOST_UVAZKU_DO")]
    public DateTime? PlatnostUvazkuDo { get; set; }
    [ColumnName("ID_NADRIZENEHO")]
    public int IdNadrizeneho { get; set; }
    [ColumnName("ID_UZIVATELE")]
    public int IdUzivatele { get; set; }

    public virtual Zamestnanci IdNadrizenehoNavigation { get; set; } = null!;

    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual ICollection<Zamestnanci> InverseIdNadrizenehoNavigation { get; set; } = new List<Zamestnanci>();
}
