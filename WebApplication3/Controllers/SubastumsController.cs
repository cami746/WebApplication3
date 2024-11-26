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
    public class SubastumsController : Controller
    {
        private readonly PlSubastas2Context _context;

        public SubastumsController(PlSubastas2Context context)
        {
            _context = context;
        }

        // GET: Subastums
        public async Task<IActionResult> Index()
        {
            var subastas = await _context.Subasta
                .Include(s => s.TipoDeSubasta)
                .Select(s => new Subastum
                {
                    SubastaId = s.SubastaId,
                    Titulo = s.Titulo,
                    Descripcion = s.Descripcion,
                    PrecioInicial = s.PrecioInicial,
                    PrecioFinal = s.PrecioFinal,
                    FechaInicio = s.FechaInicio,
                    FechaFin = s.FechaFin,
                    Estado = s.Estado,
                    TipoDeSubastaId = s.TipoDeSubastaId,
                    TipoDeSubasta = s.TipoDeSubasta
                })
                .ToListAsync();

            return View(subastas);
        }

        // GET: Subastums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subastum = await _context.Subasta
                .Include(s => s.TipoDeSubasta)
                .FirstOrDefaultAsync(m => m.SubastaId == id);

            if (subastum == null)
            {
                return NotFound();
            }

            return View(subastum);
        }

        // GET: Subastums/Create
        public IActionResult Create()
        {
            // Carga los tipos de subasta para el dropdown, mostrando el nombre
            ViewData["TipoDeSubastaId"] = new SelectList(_context.TipoDeSubasta, "TipoDeSubastaId", "Nombre");

            // Inicializa fechas por defecto
            var subastum = new Subastum
            {
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(15),
                Estado = "Activa"
            };

            return View(subastum);
        }

        // POST: Subastums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubastaId,Titulo,Descripcion,PrecioInicial,PrecioFinal,FechaInicio,FechaFin,Estado,TipoDeSubastaId")] Subastum subastum)
        {
            if (true)
            {
                // Validaciones adicionales
                if (subastum.FechaInicio >= subastum.FechaFin)
                {
                    ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio");
                    ViewData["TipoDeSubastaId"] = new SelectList(_context.TipoDeSubasta, "TipoDeSubastaId", "Nombre", subastum.TipoDeSubastaId);
                    return View(subastum);
                }

                if (subastum.PrecioInicial <= 0)
                {
                    ModelState.AddModelError("PrecioInicial", "El precio inicial debe ser mayor que 0");
                    ViewData["TipoDeSubastaId"] = new SelectList(_context.TipoDeSubasta, "TipoDeSubastaId", "Nombre", subastum.TipoDeSubastaId);
                    return View(subastum);
                }

                _context.Add(subastum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TipoDeSubastaId"] = new SelectList(_context.TipoDeSubasta, "TipoDeSubastaId", "Nombre", subastum.TipoDeSubastaId);
            return View(subastum);
        }

        // GET: Subastums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subastum = await _context.Subasta
                .Include(s => s.TipoDeSubasta)
                .FirstOrDefaultAsync(m => m.SubastaId == id);

            if (subastum == null)
            {
                return NotFound();
            }

            ViewData["TipoDeSubastaId"] = new SelectList(_context.TipoDeSubasta, "TipoDeSubastaId", "Nombre", subastum.TipoDeSubastaId);
            return View(subastum);
        }

        // POST: Subastums/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubastaId,Titulo,Descripcion,PrecioInicial,PrecioFinal,FechaInicio,FechaFin,Estado,TipoDeSubastaId")] Subastum subastum)
        {
            if (id != subastum.SubastaId)
            {
                return NotFound();
            }

            if (true)
            {
                // Validaciones adicionales
                if (subastum.FechaInicio >= subastum.FechaFin)
                {
                    ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio");
                    ViewData["TipoDeSubastaId"] = new SelectList(_context.TipoDeSubasta, "TipoDeSubastaId", "Nombre", subastum.TipoDeSubastaId);
                    return View(subastum);
                }

                if (subastum.PrecioInicial <= 0)
                {
                    ModelState.AddModelError("PrecioInicial", "El precio inicial debe ser mayor que 0");
                    ViewData["TipoDeSubastaId"] = new SelectList(_context.TipoDeSubasta, "TipoDeSubastaId", "Nombre", subastum.TipoDeSubastaId);
                    return View(subastum);
                }

                try
                {
                    _context.Update(subastum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubastumExists(subastum.SubastaId))
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

            ViewData["TipoDeSubastaId"] = new SelectList(_context.TipoDeSubasta, "TipoDeSubastaId", "Nombre", subastum.TipoDeSubastaId);
            return View(subastum);
        }

        // GET: Subastums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subastum = await _context.Subasta
                .Include(s => s.TipoDeSubasta)
                .FirstOrDefaultAsync(m => m.SubastaId == id);

            if (subastum == null)
            {
                return NotFound();
            }

            return View(subastum);
        }

        // POST: Subastums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subastum = await _context.Subasta.FindAsync(id);
            if (subastum != null)
            {
                _context.Subasta.Remove(subastum);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SubastumExists(int id)
        {
            return _context.Subasta.Any(e => e.SubastaId == id);
        }
    }
}