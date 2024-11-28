using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Logy
{
    [IdProperty]
    [ColumnName("ID_LOGU")]
    public int IdLogu { get; set; }

    public DateTime Cas { get; set; }

    public string Uzivatel { get; set; } = null!;

    public string Tabulka { get; set; } = null!;

    public string Operace { get; set; } = null!;

    public string? StaraHodnota { get; set; }

    public string? NovaHodnota { get; set; }
}
