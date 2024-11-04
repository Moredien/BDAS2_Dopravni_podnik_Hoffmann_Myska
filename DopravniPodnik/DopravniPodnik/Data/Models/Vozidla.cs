﻿using System;
using System.Collections.Generic;

namespace DopravniPodnik.Data.Models;

public partial class Vozidla
{
    public int IdVozidla { get; set; }

    public int IdTypVozidla { get; set; }

    public virtual TypyVozidel IdTypVozidlaNavigation { get; set; } = null!;

    public virtual ICollection<Jizdy> Jizdies { get; set; } = new List<Jizdy>();
}
