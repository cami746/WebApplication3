using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Inscripcion
{
    public int InscripcionId { get; set; }

    public int UsuarioId { get; set; }

    public int SubastaId { get; set; }

    public DateTime FechaInscripcion { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<Puja> Pujas { get; set; } = new List<Puja>();

    public virtual Subastum Subasta { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
