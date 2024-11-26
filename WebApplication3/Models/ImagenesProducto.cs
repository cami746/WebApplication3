using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class ImagenesProducto
{
    public int ImagenProductoId { get; set; }

    public int ProductoId { get; set; }

    public string ImagenUrl { get; set; } = null!;

    public bool? EsPrincipal { get; set; }

    public int? Orden { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
