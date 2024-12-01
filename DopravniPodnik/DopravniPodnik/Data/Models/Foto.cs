using System;
using System.Collections.Generic;
using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.Models;

public partial class Foto
{
    [IdProperty]
    [ColumnName("ID_FOTO")]
    public int? IdFoto { get; set; }
    [ColumnName("JMENO_SOUBORU")]
    public string JmenoSouboru { get; set; } = null!;

    public byte[] Data { get; set; } = null!;
    [ColumnName("DATUM_PRIDANI")]
    public DateTime DatumPridani { get; set; }
    [ColumnName("ID_KARTY")]
    public int? IdKarty { get; set; }
    [ColumnName("ID_UZIVATELE")]
    public int? IdUzivatele { get; set; }

    public virtual KartyMhd IdKartyNavigation { get; set; } = null!;

    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual KartyMhd? KartyMhd { get; set; }
}
