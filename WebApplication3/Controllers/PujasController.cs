using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class PujasController : Controller
{
    private readonly PlSubastas2Context _context;

    public PujasController(PlSubastas2Context context)
    {
        _context = context;
    }

    // GET: Pujas
    public async Task<IActionResult> Index()
    {
        var pujas = await _context.Pujas
            .Include(p => p.Inscripcion)
                .ThenInclude(i => i.Usuario)
            .Include(p => p.Inscripcion)
                .ThenInclude(i => i.Subasta)
            .OrderByDescending(p => p.FechaPuja)
            .ToListAsync();

        return View(pujas);
    }

    // GET: Pujas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var puja = await _context.Pujas
            .Include(p => p.Inscripcion)
                .ThenInclude(i => i.Usuario)
            .Include(p => p.Inscripcion)
                .ThenInclude(i => i.Subasta)
            .FirstOrDefaultAsync(m => m.PujaId == id);

        if (puja == null)
        {
            return NotFound();
        }

        return View(puja);
    }

    // GET: Pujas/Create
    public IActionResult Create()
    {
        var inscripciones = _context.Inscripcions
            .Include(i => i.Usuario)
            .Include(i => i.Subasta)
            .Select(i => new
            {
                i.InscripcionId,
                Descripcion = $"{i.Usuario.Nombre} {i.Usuario.Apellido} - {i.Subasta.Titulo}"
            })
            .ToList();

        ViewData["InscripcionId"] = new SelectList(inscripciones, "InscripcionId", "Descripcion");
        return View();
    }

    // POST: Pujas/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Monto,InscripcionId")] Puja puja)
    {
        if (true)
        {
            try
            {
                puja.FechaPuja = DateTime.Now;
                _context.Add(puja);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Puja registrada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al registrar la puja: " + ex.Message);
            }
        }

        var inscripciones = _context.Inscripcions
            .Include(i => i.Usuario)
            .Include(i => i.Subasta)
            .Select(i => new
            {
                i.InscripcionId,
                Descripcion = $"{i.Usuario.Nombre} {i.Usuario.Apellido} - {i.Subasta.Titulo}"
            })
            .ToList();

        ViewData["InscripcionId"] = new SelectList(inscripciones, "InscripcionId", "Descripcion", puja.InscripcionId);
        return View(puja);
    }

    // GET: Pujas/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var puja = await _context.Pujas
            .Include(p => p.Inscripcion)
                .ThenInclude(i => i.Usuario)
            .Include(p => p.Inscripcion)
                .ThenInclude(i => i.Subasta)
            .FirstOrDefaultAsync(p => p.PujaId == id);

        if (puja == null)
        {
            return NotFound();
        }

        var inscripciones = _context.Inscripcions
            .Include(i => i.Usuario)
            .Include(i => i.Subasta)
            .Select(i => new
            {
                i.InscripcionId,
                Descripcion = $"{i.Usuario.Nombre} {i.Usuario.Apellido} - {i.Subasta.Titulo}"
            })
            .ToList();

        ViewData["InscripcionId"] = new SelectList(inscripciones, "InscripcionId", "Descripcion", puja.InscripcionId);
        return View(puja);
    }

    // POST: Pujas/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PujaId,Monto,FechaPuja,InscripcionId")] Puja puja)
    {
        if (id != puja.PujaId)
        {
            return NotFound();
        }

        if (true)
        {
            try
            {
                _context.Update(puja);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Puja actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PujaExists(puja.PujaId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        var inscripciones = _context.Inscripcions
            .Include(i => i.Usuario)
            .Include(i => i.Subasta)
            .Select(i => new
            {
                i.InscripcionId,
                Descripcion = $"{i.Usuario.Nombre} {i.Usuario.Apellido} - {i.Subasta.Titulo}"
            })
            .ToList();

        ViewData["InscripcionId"] = new SelectList(inscripciones, "InscripcionId", "Descripcion", puja.InscripcionId);
        return View(puja);
    }

    // GET: Pujas/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var puja = await _context.Pujas
            .Include(p => p.Inscripcion)
                .ThenInclude(i => i.Usuario)
            .Include(p => p.Inscripcion)
                .ThenInclude(i => i.Subasta)
            .FirstOrDefaultAsync(m => m.PujaId == id);

        if (puja == null)
        {
            return NotFound();
        }

        return View(puja);
    }

    // POST: Pujas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var puja = await _context.Pujas.FindAsync(id);
            if (puja != null)
            {
                _context.Pujas.Remove(puja);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Puja eliminada exitosamente.";
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al eliminar la puja: " + ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PujaExists(int id)
    {
        return _context.Pujas.Any(e => e.PujaId == id);
    }
}
