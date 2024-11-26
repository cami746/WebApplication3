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
    public class InscripcionsController : Controller
    {
        private readonly PlSubastas2Context _context;

        public InscripcionsController(PlSubastas2Context context)
        {
            _context = context;
        }

        // Modelo de vista para mostrar la información formateada
        public class InscripcionViewModel
        {
            public int InscripcionId { get; set; }
            public DateTime FechaInscripcion { get; set; }
            public string NombreUsuario { get; set; }
            public string TituloSubasta { get; set; }
            public int UsuarioId { get; set; }
            public int SubastaId { get; set; }
        }

        // GET: Inscripcions
        public async Task<IActionResult> Index()
        {
            var inscripciones = await _context.Inscripcions
                .Include(i => i.Subasta)
                .Include(i => i.Usuario)
                .Select(i => new InscripcionViewModel
                {
                    InscripcionId = i.InscripcionId,
                    FechaInscripcion = i.FechaInscripcion,
                    NombreUsuario = i.Usuario.Nombre,
                    TituloSubasta = i.Subasta.Titulo,
                    UsuarioId = i.UsuarioId,
                    SubastaId = i.SubastaId
                })
                .ToListAsync();

            return View(inscripciones);
        }

        // GET: Inscripcions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcions
                .Include(i => i.Subasta)
                .Include(i => i.Usuario)
                .Select(i => new InscripcionViewModel
                {
                    InscripcionId = i.InscripcionId,
                    FechaInscripcion = i.FechaInscripcion,
                    NombreUsuario = i.Usuario.Nombre,
                    TituloSubasta = i.Subasta.Titulo,
                    UsuarioId = i.UsuarioId,
                    SubastaId = i.SubastaId
                })
                .FirstOrDefaultAsync(m => m.InscripcionId == id);

            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        // GET: Inscripcions/Create
        public IActionResult Create()
        {
            // Modificamos los SelectList para mostrar información descriptiva
            ViewData["SubastaId"] = new SelectList(_context.Subasta, "SubastaId", "Titulo");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: Inscripcions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InscripcionId,UsuarioId,SubastaId,FechaInscripcion")] Inscripcion inscripcion)
        {
            if (true)
            {
                inscripcion.FechaInscripcion = DateTime.Now; // Asignamos la fecha actual
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // En caso de error, volvemos a cargar las listas con nombres descriptivos
            ViewData["SubastaId"] = new SelectList(_context.Subasta, "SubastaId", "Titulo", inscripcion.SubastaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", inscripcion.UsuarioId);
            return View(inscripcion);
        }

        // GET: Inscripcions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcions.FindAsync(id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            // Cargamos las listas con nombres descriptivos
            ViewData["SubastaId"] = new SelectList(_context.Subasta, "SubastaId", "Titulo", inscripcion.SubastaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", inscripcion.UsuarioId);
            return View(inscripcion);
        }

        // POST: Inscripcions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InscripcionId,UsuarioId,SubastaId,FechaInscripcion")] Inscripcion inscripcion)
        {
            if (id != inscripcion.InscripcionId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(inscripcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InscripcionExists(inscripcion.InscripcionId))
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

            // En caso de error, volvemos a cargar las listas con nombres descriptivos
            ViewData["SubastaId"] = new SelectList(_context.Subasta, "SubastaId", "Titulo", inscripcion.SubastaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", inscripcion.UsuarioId);
            return View(inscripcion);
        }

        // GET: Inscripcions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripcions
                .Include(i => i.Subasta)
                .Include(i => i.Usuario)
                .Select(i => new InscripcionViewModel
                {
                    InscripcionId = i.InscripcionId,
                    FechaInscripcion = i.FechaInscripcion,
                    NombreUsuario = i.Usuario.Nombre,
                    TituloSubasta = i.Subasta.Titulo,
                    UsuarioId = i.UsuarioId,
                    SubastaId = i.SubastaId
                })
                .FirstOrDefaultAsync(m => m.InscripcionId == id);

            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        // POST: Inscripcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscripcion = await _context.Inscripcions.FindAsync(id);
            if (inscripcion != null)
            {
                _context.Inscripcions.Remove(inscripcion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscripcionExists(int id)
        {
            return _context.Inscripcions.Any(e => e.InscripcionId == id);
        }
    }
}