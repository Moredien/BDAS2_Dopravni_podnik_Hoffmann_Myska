using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Foto
{
    public int IdFoto { get; set; }

    public string JmenoSouboru { get; set; } = null!;

    public byte[] Data { get; set; } = null!;

    public DateTime DatumPridani { get; set; }

    public int IdKarty { get; set; }

    public int IdUzivatele { get; set; }

    public virtual KartyMhd IdKartyNavigation { get; set; } = null!;

    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual KartyMhd? KartyMhd { get; set; }
}
