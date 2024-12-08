using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class KartyMhd
{
    [IdProperty]
    [ColumnName("ID_KARTY")]
    public int IdKarty { get; set; }

    public int Zustatek { get; set; }
    [ColumnName("PLATNOST_OD")]
    public DateTime PlatnostOd { get; set; }
    [ColumnName("PLATNOST_DO")]
    public DateTime PlatnostDo { get; set; }
    [ColumnName("ID_ZAKAZNIKA")]
    public int IdZakaznika { get; set; }
    [ColumnName("ID_FOTO")]
    public int IdFoto { get; set; }
}
