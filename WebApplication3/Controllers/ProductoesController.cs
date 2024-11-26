using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ProductoesController : Controller
    {
        private readonly PlSubastas2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoesController(PlSubastas2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Productoes
        public async Task<IActionResult> Index(string searchString, int? categoriaId)
        {
            var query = _context.Productos
                .Include(p => p.CategoriaProducto)
                .Include(p => p.Subasta)
                .Include(p => p.ImagenesProductos)
                .AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Nombre.Contains(searchString) ||
                                       p.Descripcion.Contains(searchString));
            }

            if (categoriaId.HasValue)
            {
                query = query.Where(p => p.CategoriaProductoId == categoriaId);
            }

            // Cargar categorías para el filtro
            ViewBag.Categorias = new SelectList(await _context.CategoriaProductos.ToListAsync(),
                "CategoriaProductoId", "Nombre");
            ViewBag.CurrentFilter = searchString;
            ViewBag.CategoriaId = categoriaId;

            return View(await query.ToListAsync());
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.CategoriaProducto)
                .Include(p => p.Subasta)
                .Include(p => p.ImagenesProductos)
                .FirstOrDefaultAsync(m => m.ProductoId == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewBag.CategoriaProductoId = new SelectList(_context.CategoriaProductos,
                "CategoriaProductoId", "Nombre");
            ViewBag.SubastaId = new SelectList(_context.Subasta
                .Where(s => s.Estado == "Activa"),
                "SubastaId", "Titulo");
            return View();
        }

        // POST: Productoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Precio,CategoriaProductoId,SubastaId")]
           Producto producto, IFormFile imagenPrincipal, List<IFormFile> imagenesAdicionales)
        {
            if (true)
            {
                try
                {
                    // Procesar imagen principal
                    if (imagenPrincipal != null)
                    {
                        var rutaImagen = await GuardarImagen(imagenPrincipal);
                        if (!string.IsNullOrEmpty(rutaImagen))
                        {
                            producto.ImagenPrincipalUrl = rutaImagen;
                        }
                    }

                    // Guardar el producto
                    _context.Add(producto);
                    await _context.SaveChangesAsync();

                    // Procesar imágenes adicionales
                    if (imagenesAdicionales != null && imagenesAdicionales.Any())
                    {
                        foreach (var imagen in imagenesAdicionales)
                        {
                            var rutaImagen = await GuardarImagen(imagen);
                            if (!string.IsNullOrEmpty(rutaImagen))
                            {
                                var imagenProducto = new ImagenesProducto
                                {
                                    ProductoId = producto.ProductoId,
                                    ImagenUrl = rutaImagen,
                                    EsPrincipal = false
                                };
                                _context.ImagenesProductos.Add(imagenProducto);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                    TempData["Mensaje"] = "Producto creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear el producto: " + ex.Message);
                }
            }

            ViewBag.CategoriaProductoId = new SelectList(_context.CategoriaProductos,
                "CategoriaProductoId", "Nombre", producto.CategoriaProductoId);
            ViewBag.SubastaId = new SelectList(_context.Subasta.Where(s => s.Estado == "Activa"),
                "SubastaId", "Titulo", producto.SubastaId);
            return View(producto);
        }

        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.ImagenesProductos)
                .FirstOrDefaultAsync(p => p.ProductoId == id);

            if (producto == null)
            {
                return NotFound();
            }

            ViewBag.CategoriaProductoId = new SelectList(_context.CategoriaProductos,
                "CategoriaProductoId", "Nombre", producto.CategoriaProductoId);
            ViewBag.SubastaId = new SelectList(_context.Subasta.Where(s => s.Estado == "Activa"),
                "SubastaId", "Titulo", producto.SubastaId);
            return View(producto);
        }

        // POST: Productoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Descripcion,Precio,CategoriaProductoId,SubastaId,ImagenPrincipalUrl")]
           Producto producto, IFormFile nuevaImagenPrincipal, List<IFormFile> imagenesAdicionales, List<int> imagenesAEliminar)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    // Procesar nueva imagen principal
                    if (nuevaImagenPrincipal != null)
                    {
                        var rutaImagen = await GuardarImagen(nuevaImagenPrincipal);
                        if (!string.IsNullOrEmpty(rutaImagen))
                        {
                            // Eliminar imagen anterior si existe
                            if (!string.IsNullOrEmpty(producto.ImagenPrincipalUrl))
                            {
                                EliminarImagen(producto.ImagenPrincipalUrl);
                            }
                            producto.ImagenPrincipalUrl = rutaImagen;
                        }
                    }

                    // Eliminar imágenes seleccionadas
                    if (imagenesAEliminar != null && imagenesAEliminar.Any())
                    {
                        foreach (var imagenId in imagenesAEliminar)
                        {
                            var imagen = await _context.ImagenesProductos
                                .FirstOrDefaultAsync(i => i.ImagenProductoId == imagenId);
                            if (imagen != null)
                            {
                                EliminarImagen(imagen.ImagenUrl);
                                _context.ImagenesProductos.Remove(imagen);
                            }
                        }
                    }

                    // Agregar nuevas imágenes
                    if (imagenesAdicionales != null && imagenesAdicionales.Any())
                    {
                        foreach (var imagen in imagenesAdicionales)
                        {
                            var rutaImagen = await GuardarImagen(imagen);
                            if (!string.IsNullOrEmpty(rutaImagen))
                            {
                                var imagenProducto = new ImagenesProducto
                                {
                                    ProductoId = producto.ProductoId,
                                    ImagenUrl = rutaImagen,
                                    EsPrincipal = false
                                };
                                _context.ImagenesProductos.Add(imagenProducto);
                            }
                        }
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Producto actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.ProductoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.CategoriaProductoId = new SelectList(_context.CategoriaProductos,
                "CategoriaProductoId", "Nombre", producto.CategoriaProductoId);
            ViewBag.SubastaId = new SelectList(_context.Subasta.Where(s => s.Estado == "Activa"),
                "SubastaId", "Titulo", producto.SubastaId);
            return View(producto);
        }

        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.CategoriaProducto)
                .Include(p => p.Subasta)
                .Include(p => p.ImagenesProductos)
                .FirstOrDefaultAsync(m => m.ProductoId == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var producto = await _context.Productos
                    .Include(p => p.ImagenesProductos)
                    .FirstOrDefaultAsync(p => p.ProductoId == id);

                if (producto != null)
                {
                    // Eliminar imagen principal
                    if (!string.IsNullOrEmpty(producto.ImagenPrincipalUrl))
                    {
                        EliminarImagen(producto.ImagenPrincipalUrl);
                    }

                    // Eliminar imágenes adicionales
                    if (producto.ImagenesProductos != null)
                    {
                        foreach (var imagen in producto.ImagenesProductos.ToList())
                        {
                            EliminarImagen(imagen.ImagenUrl);
                            _context.ImagenesProductos.Remove(imagen);
                        }
                    }

                    // Eliminar el producto
                    _context.Productos.Remove(producto);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["Mensaje"] = "Producto eliminado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["Error"] = "Error al eliminar el producto: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }

        private async Task<string> GuardarImagen(IFormFile imagen)
        {
            if (imagen != null && imagen.Length > 0)
            {
                try
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "productos");

                    // Crear el directorio si no existe
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Crear un nombre único para el archivo
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" +
                        Path.GetFileName(imagen.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Guardar el archivo
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagen.CopyToAsync(fileStream);
                    }

                    // Devolver la ruta relativa
                    return $"~/images/productos/{uniqueFileName}";
                }
                catch (Exception ex)
                {
                    // Loguear el error
                    Console.WriteLine($"Error al guardar imagen: {ex.Message}");
                    return null;
                }
            }
            return null;
        }

        private void EliminarImagen(string rutaImagen)
        {
            if (!string.IsNullOrEmpty(rutaImagen))
            {
                try
                {
                    // Convertir ruta relativa a absoluta
                    var rutaCompleta = Path.Combine(_webHostEnvironment.WebRootPath,
                        rutaImagen.TrimStart('~', '/'));

                    if (System.IO.File.Exists(rutaCompleta))
                    {
                        System.IO.File.Delete(rutaCompleta);
                    }
                }
                catch (Exception ex)
                {
                    // Loguear el error
                    Console.WriteLine($"Error al eliminar imagen: {ex.Message}");
                }
            }
        }
    }
}