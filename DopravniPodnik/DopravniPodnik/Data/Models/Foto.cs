using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Foto
{
    public decimal IdFoto { get; set; }

    public string JmenoSouboru { get; set; } = null!;

    public byte[] Data { get; set; } = null!;

    public DateTime DatumPridani { get; set; }

    public decimal KartaMhdIdKarty { get; set; }

    public decimal UzivateleIdUzivatele { get; set; }

    public virtual KartyMhd KartaMhdIdKartyNavigation { get; set; } = null!;

    public virtual KartyMhd? KartyMhd { get; set; }

    public virtual Uzivatele UzivateleIdUzivateleNavigation { get; set; } = null!;
}
