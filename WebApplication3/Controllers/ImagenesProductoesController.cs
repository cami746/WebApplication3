using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ImagenesProductoesController : Controller
    {
        private readonly PlSubastas2Context _context;

        public ImagenesProductoesController(PlSubastas2Context context)
        {
            _context = context;
        }

        // GET: ImagenesProductoes
        public async Task<IActionResult> Index()
        {
            var plSubastas2Context = _context.ImagenesProductos.Include(i => i.Producto);
            return View(await plSubastas2Context.ToListAsync());
        }

        // GET: ImagenesProductoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagenesProducto = await _context.ImagenesProductos
                .Include(i => i.Producto)
                .FirstOrDefaultAsync(m => m.ImagenProductoId == id);
            if (imagenesProducto == null)
            {
                return NotFound();
            }

            return View(imagenesProducto);
        }

        // GET: ImagenesProductoes/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId");
            return View();
        }

        // POST: ImagenesProductoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImagenProductoId,ProductoId,ImagenUrl,EsPrincipal,Orden")] ImagenesProducto imagenesProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imagenesProducto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", imagenesProducto.ProductoId);
            return View(imagenesProducto);
        }

        // GET: ImagenesProductoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagenesProducto = await _context.ImagenesProductos.FindAsync(id);
            if (imagenesProducto == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", imagenesProducto.ProductoId);
            return View(imagenesProducto);
        }

        // POST: ImagenesProductoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImagenProductoId,ProductoId,ImagenUrl,EsPrincipal,Orden")] ImagenesProducto imagenesProducto)
        {
            if (id != imagenesProducto.ImagenProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagenesProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagenesProductoExists(imagenesProducto.ImagenProductoId))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", imagenesProducto.ProductoId);
            return View(imagenesProducto);
        }

        // GET: ImagenesProductoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagenesProducto = await _context.ImagenesProductos
                .Include(i => i.Producto)
                .FirstOrDefaultAsync(m => m.ImagenProductoId == id);
            if (imagenesProducto == null)
            {
                return NotFound();
            }

            return View(imagenesProducto);
        }

        // POST: ImagenesProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imagenesProducto = await _context.ImagenesProductos.FindAsync(id);
            if (imagenesProducto != null)
            {
                _context.ImagenesProductos.Remove(imagenesProducto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagenesProductoExists(int id)
        {
            return _context.ImagenesProductos.Any(e => e.ImagenProductoId == id);
        }
    }
}
