using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class SubcategoriaProducto
{
    public int SubcategoriaProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public int CategoriaProductoId { get; set; }

    public string? ImagenUrl { get; set; }

    public virtual CategoriaProducto CategoriaProducto { get; set; } = null!;
}
