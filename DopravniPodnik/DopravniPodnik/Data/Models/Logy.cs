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
    [ColumnName("STARA_HODNOTA")]
    public string? StaraHodnota { get; set; }
    [ColumnName("NOVA_HODNOTA")]
    public string? NovaHodnota { get; set; }
}
