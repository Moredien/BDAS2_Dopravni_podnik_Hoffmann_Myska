using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Logy
{
    public int IdLogu { get; set; }

    public DateTime Cas { get; set; }

    public string Uzivatel { get; set; } = null!;

    public string Tabulka { get; set; } = null!;

    public string Operace { get; set; } = null!;

    public string? StaraHodnota { get; set; }

    public string? NovaHodnota { get; set; }
}
