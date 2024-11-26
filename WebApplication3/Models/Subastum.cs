using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Subastum
{
    public int SubastaId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal PrecioInicial { get; set; }

    public decimal? PrecioFinal { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Estado { get; set; } = null!;

    public int TipoDeSubastaId { get; set; }

    public virtual ICollection<Inscripcion> Inscripcions { get; set; } = new List<Inscripcion>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual TipoDeSubastum TipoDeSubasta { get; set; } = null!;
 
}
