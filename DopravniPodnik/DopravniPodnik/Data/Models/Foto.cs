using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Foto
{
    public decimal IdFoto { get; set; }

    public string JmenoSouboru { get; set; } = null!;

    public byte[] Data { get; set; } = null!;

    public DateTime DatumPridani { get; set; }

    public decimal IdKarty { get; set; }

    public decimal IdUzivatele { get; set; }
    
    public virtual Uzivatele IdUzivateleNavigation { get; set; } = null!;

    public virtual KartyMhd? KartyMhd { get; set; }
}
