using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class TipoDeSubastum
{
    public int TipoDeSubastaId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Subastum> Subasta { get; set; } = new List<Subastum>();
}
