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
    public class PagoesController : Controller
    {
        private readonly PlSubastas2Context _context;

        public PagoesController(PlSubastas2Context context)
        {
            _context = context;
        }

        // GET: Pagoes
        public async Task<IActionResult> Index()
        {
            var plSubastas2Context = _context.Pagos
                .Include(p => p.Inscripcion)
                .Include(p => p.Inscripcion.Usuario);
            return View(await plSubastas2Context.ToListAsync());
        }

        // GET: Pagoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Inscripcion)
                .Include(p => p.Inscripcion.Usuario)
                .FirstOrDefaultAsync(m => m.PagoId == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagoes/Create
        public IActionResult Create()
        {
            ViewData["InscripcionId"] = new SelectList(_context.Inscripcions
                .Include(i => i.Usuario)
                .Select(i => new
                {
                    i.InscripcionId,
                    NombreCompleto = $"{i.Usuario.Nombre} {i.Usuario.Apellido}"
                }),
                "InscripcionId",
                "NombreCompleto");
            return View();
        }

        // POST: Pagoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PagoId,Monto,FechaPago,InscripcionId")] Pago pago)
        {
            if (true)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InscripcionId"] = new SelectList(_context.Inscripcions
                .Include(i => i.Usuario)
                .Select(i => new
                {
                    i.InscripcionId,
                    NombreCompleto = $"{i.Usuario.Nombre} {i.Usuario.Apellido}"
                }),
                "InscripcionId",
                "NombreCompleto",
                pago.InscripcionId);
            return View(pago);
        }

        // GET: Pagoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["InscripcionId"] = new SelectList(_context.Inscripcions
                .Include(i => i.Usuario)
                .Select(i => new
                {
                    i.InscripcionId,
                    NombreCompleto = $"{i.Usuario.Nombre} {i.Usuario.Apellido}"
                }),
                "InscripcionId",
                "NombreCompleto",
                pago.InscripcionId);
            return View(pago);
        }

        // POST: Pagoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PagoId,Monto,FechaPago,InscripcionId")] Pago pago)
        {
            if (id != pago.PagoId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.PagoId))
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
            ViewData["InscripcionId"] = new SelectList(_context.Inscripcions
                .Include(i => i.Usuario)
                .Select(i => new
                {
                    i.InscripcionId,
                    NombreCompleto = $"{i.Usuario.Nombre} {i.Usuario.Apellido}"
                }),
                "InscripcionId",
                "NombreCompleto",
                pago.InscripcionId);
            return View(pago);
        }

        // GET: Pagoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Inscripcion)
                .Include(p => p.Inscripcion.Usuario)
                .FirstOrDefaultAsync(m => m.PagoId == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                _context.Pagos.Remove(pago);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.PagoId == id);
        }
    }
}
