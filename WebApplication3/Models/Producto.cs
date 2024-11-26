using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int CategoriaProductoId { get; set; }

    public int SubastaId { get; set; }

    public string? ImagenPrincipalUrl { get; set; }

    public virtual CategoriaProducto CategoriaProducto { get; set; } = null!;

    public virtual ICollection<ImagenesProducto> ImagenesProductos { get; set; } = new List<ImagenesProducto>();

    public virtual Subastum Subasta { get; set; } = null!;
}
