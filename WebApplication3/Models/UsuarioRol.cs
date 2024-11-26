using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class UsuarioRol
{
    public int UsuarioRolId { get; set; }

    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public DateTime? FechaAsignacion { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
