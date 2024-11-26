using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

public class SubcategoriaProductoesController : Controller
{
    private readonly PlSubastas2Context _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SubcategoriaProductoesController(PlSubastas2Context context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: SubcategoriaProductoes
    public async Task<IActionResult> Index()
    {
        var subcategorias = await _context.SubcategoriaProductos
            .Include(s => s.CategoriaProducto)
            .OrderBy(s => s.CategoriaProducto.Nombre)
            .ThenBy(s => s.Nombre)
            .ToListAsync();
        return View(subcategorias);
    }

    // GET: SubcategoriaProductoes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subcategoriaProducto = await _context.SubcategoriaProductos
            .Include(s => s.CategoriaProducto)
            .FirstOrDefaultAsync(m => m.SubcategoriaProductoId == id);

        if (subcategoriaProducto == null)
        {
            return NotFound();
        }

        return View(subcategoriaProducto);
    }

    // GET: SubcategoriaProductoes/Create
    public IActionResult Create()
    {
        ViewData["CategoriaProductoId"] = new SelectList(
            _context.CategoriaProductos.OrderBy(c => c.Nombre),
            "CategoriaProductoId",
            "Nombre"
        );
        return View();
    }

    // POST: SubcategoriaProductoes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,CategoriaProductoId")] SubcategoriaProducto subcategoriaProducto,
        IFormFile imagen)
    {
        if (true)
        {
            try
            {
                // Procesar imagen
                if (imagen != null)
                {
                    subcategoriaProducto.ImagenUrl = await GuardarImagen(imagen);
                }

                _context.Add(subcategoriaProducto);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Subcategoría creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear la subcategoría: " + ex.Message);
            }
        }

        ViewData["CategoriaProductoId"] = new SelectList(
            _context.CategoriaProductos.OrderBy(c => c.Nombre),
            "CategoriaProductoId",
            "Nombre",
            subcategoriaProducto.CategoriaProductoId
        );
        return View(subcategoriaProducto);
    }

    // GET: SubcategoriaProductoes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subcategoriaProducto = await _context.SubcategoriaProductos
            .Include(s => s.CategoriaProducto)
            .FirstOrDefaultAsync(s => s.SubcategoriaProductoId == id);

        if (subcategoriaProducto == null)
        {
            return NotFound();
        }

        ViewData["CategoriaProductoId"] = new SelectList(
            _context.CategoriaProductos.OrderBy(c => c.Nombre),
            "CategoriaProductoId",
            "Nombre",
            subcategoriaProducto.CategoriaProductoId
        );
        return View(subcategoriaProducto);
    }

    // POST: SubcategoriaProductoes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("SubcategoriaProductoId,Nombre,CategoriaProductoId,ImagenUrl")]
        SubcategoriaProducto subcategoriaProducto, IFormFile nuevaImagen)
    {
        if (id != subcategoriaProducto.SubcategoriaProductoId)
        {
            return NotFound();
        }

        if (true)
        {
            try
            {
                // Procesar nueva imagen
                if (nuevaImagen != null)
                {
                    // Eliminar imagen anterior
                    if (!string.IsNullOrEmpty(subcategoriaProducto.ImagenUrl))
                    {
                        EliminarImagen(subcategoriaProducto.ImagenUrl);
                    }
                    subcategoriaProducto.ImagenUrl = await GuardarImagen(nuevaImagen);
                }

                _context.Update(subcategoriaProducto);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Subcategoría actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubcategoriaProductoExists(subcategoriaProducto.SubcategoriaProductoId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar la subcategoría: " + ex.Message);
            }
        }

        ViewData["CategoriaProductoId"] = new SelectList(
            _context.CategoriaProductos.OrderBy(c => c.Nombre),
            "CategoriaProductoId",
            "Nombre",
            subcategoriaProducto.CategoriaProductoId
        );
        return View(subcategoriaProducto);
    }

    // GET: SubcategoriaProductoes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subcategoriaProducto = await _context.SubcategoriaProductos
            .Include(s => s.CategoriaProducto)
            .FirstOrDefaultAsync(m => m.SubcategoriaProductoId == id);

        if (subcategoriaProducto == null)
        {
            return NotFound();
        }

        return View(subcategoriaProducto);
    }

    // POST: SubcategoriaProductoes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subcategoriaProducto = await _context.SubcategoriaProductos.FindAsync(id);
        if (subcategoriaProducto != null)
        {
            try
            {
                // Eliminar imagen si existe
                if (!string.IsNullOrEmpty(subcategoriaProducto.ImagenUrl))
                {
                    EliminarImagen(subcategoriaProducto.ImagenUrl);
                }

                _context.SubcategoriaProductos.Remove(subcategoriaProducto);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Subcategoría eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar la subcategoría: " + ex.Message;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    private bool SubcategoriaProductoExists(int id)
    {
        return _context.SubcategoriaProductos.Any(e => e.SubcategoriaProductoId == id);
    }

    private async Task<string> GuardarImagen(IFormFile imagen)
    {
        if (imagen != null && imagen.Length > 0)
        {
            try
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "subcategorias");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" +
                    Path.GetFileName(imagen.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imagen.CopyToAsync(fileStream);
                }

                return $"~/images/subcategorias/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la imagen: " + ex.Message);
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
                var rutaCompleta = Path.Combine(_webHostEnvironment.WebRootPath,
                    rutaImagen.TrimStart('~', '/'));
                if (System.IO.File.Exists(rutaCompleta))
                {
                    System.IO.File.Delete(rutaCompleta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la imagen: " + ex.Message);
            }
        }
    }
}