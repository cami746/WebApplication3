using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication3.Controllers
{
    public class CategoriaProductoesController : Controller
    {
        private readonly PlSubastas2Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriaProductoesController(PlSubastas2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: CategoriaProductoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoriaProductos.ToListAsync());
        }

        // GET: CategoriaProductoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaProducto = await _context.CategoriaProductos
                .FirstOrDefaultAsync(m => m.CategoriaProductoId == id);
            if (categoriaProducto == null)
            {
                return NotFound();
            }

            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaProductoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaProductoId,Nombre,Descripcion,ImagenUrl")] CategoriaProducto categoriaProducto, IFormFile ImagenFile)
        {
            if (ModelState.IsValid)
            {
                if (ImagenFile != null && ImagenFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "categorias");

                    // Crear el directorio si no existe
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Crear nombre único para el archivo
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagenFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Guardar el archivo
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImagenFile.CopyToAsync(stream);
                    }

                    // Actualizar la URL de la imagen en el modelo
                    categoriaProducto.ImagenUrl = "/images/categorias/" + uniqueFileName;
                }

                _context.Add(categoriaProducto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaProducto = await _context.CategoriaProductos.FindAsync(id);
            if (categoriaProducto == null)
            {
                return NotFound();
            }
            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaProductoId,Nombre,Descripcion,ImagenUrl")] CategoriaProducto categoriaProducto, IFormFile ImagenFile)
        {
            if (id != categoriaProducto.CategoriaProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Si hay un archivo nuevo
                    if (ImagenFile != null && ImagenFile.Length > 0)
                    {
                        // Eliminar imagen anterior si existe
                        if (!string.IsNullOrEmpty(categoriaProducto.ImagenUrl))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                categoriaProducto.ImagenUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "categorias");

                        // Crear el directorio si no existe
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Crear nombre único para el archivo
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagenFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Guardar el archivo
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImagenFile.CopyToAsync(stream);
                        }

                        // Actualizar la URL de la imagen en el modelo
                        categoriaProducto.ImagenUrl = "/images/categorias/" + uniqueFileName;
                    }

                    _context.Update(categoriaProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaProductoExists(categoriaProducto.CategoriaProductoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoriaProducto);
        }

        // GET: CategoriaProductoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriaProducto = await _context.CategoriaProductos
                .FirstOrDefaultAsync(m => m.CategoriaProductoId == id);
            if (categoriaProducto == null)
            {
                return NotFound();
            }

            return View(categoriaProducto);
        }

        // POST: CategoriaProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoriaProducto = await _context.CategoriaProductos.FindAsync(id);
            if (categoriaProducto != null)
            {
                // Eliminar la imagen si existe
                if (!string.IsNullOrEmpty(categoriaProducto.ImagenUrl))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                        categoriaProducto.ImagenUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.CategoriaProductos.Remove(categoriaProducto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaProductoExists(int id)
        {
            return _context.CategoriaProductos.Any(e => e.CategoriaProductoId == id);
        }
    }
}
