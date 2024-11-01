using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Uzivatele
{
    public decimal IdUzivatele { get; set; }

    public string UzivatelskeJmeno { get; set; } = null!;

    public string Heslo { get; set; } = null!;

    public string Jmeno { get; set; } = null!;

    public string Prijmeni { get; set; } = null!;

    public DateTime CasZalozeni { get; set; }

    public DateTime DatumNarozeni { get; set; }

    public decimal IdTypUzivatele { get; set; }

    public decimal IdAdresy { get; set; }

    public virtual ICollection<Foto> Fotos { get; set; } = new List<Foto>();

    public virtual Adresy IdAdresyNavigation { get; set; } = null!;

    public virtual TypyUzivatele IdTypUzivateleNavigation { get; set; } = null!;

    public virtual Zakaznici? Zakaznici { get; set; }

    public virtual Zamestnanci? Zamestnanci { get; set; }
}
