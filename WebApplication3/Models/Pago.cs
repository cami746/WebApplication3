using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public decimal Monto { get; set; }

    public DateTime FechaPago { get; set; }

    public int InscripcionId { get; set; }

    public virtual Inscripcion Inscripcion { get; set; } = null!;
}
