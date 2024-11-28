using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Uzivatele
{
    [IdProperty]
    [ColumnName("ID_UZIVATELE")]
    public int IdUzivatele { get; set; }

    public string UzivatelskeJmeno { get; set; } = null!;

    public string Heslo { get; set; } = null!;

    public string Jmeno { get; set; } = null!;

    public string Prijmeni { get; set; } = null!;

    public DateTime CasZalozeni { get; set; }

    public DateTime DatumNarozeni { get; set; }

    public int IdTypUzivatele { get; set; }

    public int IdAdresy { get; set; }

    public virtual ICollection<Foto> Fotos { get; set; } = new List<Foto>();

    public virtual Adresy IdAdresyNavigation { get; set; } = null!;

    public virtual TypyUzivatele IdTypUzivateleNavigation { get; set; } = null!;

    public virtual Zakaznici? Zakaznici { get; set; }

    public virtual Zamestnanci? Zamestnanci { get; set; }
}
