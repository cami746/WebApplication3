using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class CategoriaProducto
{
    public int CategoriaProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? ImagenUrl { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<SubcategoriaProducto> SubcategoriaProductos { get; set; } = new List<SubcategoriaProducto>();
}
