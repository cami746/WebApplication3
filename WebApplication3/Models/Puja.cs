using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Puja
{
    public int PujaId { get; set; }

    public decimal Monto { get; set; }

    public DateTime FechaPuja { get; set; }

    public int InscripcionId { get; set; }

    public virtual Inscripcion Inscripcion { get; set; } = null!;
}
