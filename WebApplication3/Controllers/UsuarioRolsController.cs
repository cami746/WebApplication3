using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class UsuarioRolsController : Controller
    {
        private readonly PlSubastas2Context _context;

        public UsuarioRolsController(PlSubastas2Context context)
        {
            _context = context;
        }

        // GET: UsuarioRols
        public async Task<IActionResult> Index()
        {
            var usuarioRoles = await _context.UsuarioRols
                .Include(u => u.IdUsuarioNavigation)
                .Include(u => u.IdRolNavigation)
                .OrderBy(u => u.IdUsuarioNavigation.Nombre)
                .ThenBy(u => u.IdUsuarioNavigation.Apellido)
                .ToListAsync();

            return View(usuarioRoles);
        }

        // GET: UsuarioRols/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioRol = await _context.UsuarioRols
                .Include(u => u.IdRolNavigation)
                .Include(u => u.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.UsuarioRolId == id);

            if (usuarioRol == null)
            {
                return NotFound();
            }

            return View(usuarioRol);
        }

        // GET: UsuarioRols/Create
        public IActionResult Create()
        {
            var usuarios = _context.Usuarios
                .OrderBy(u => u.Nombre)
                .ThenBy(u => u.Apellido)
                .Select(u => new
                {
                    IdUsuario = u.IdUsuario,
                    NombreCompleto = $"{u.Nombre} {u.Apellido}"
                });

            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "NombreCompleto");
            ViewData["IdRol"] = new SelectList(_context.Rols.OrderBy(r => r.Nombre), "IdRol", "Nombre");

            return View();
        }

        // POST: UsuarioRols/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,IdRol")] UsuarioRol usuarioRol)
        {
            if (true)
            {
                try
                {
                    // Verificar si ya existe la asignación
                    var existeAsignacion = await _context.UsuarioRols
                        .AnyAsync(ur => ur.IdUsuario == usuarioRol.IdUsuario &&
                                      ur.IdRol == usuarioRol.IdRol);

                    if (existeAsignacion)
                    {
                        ModelState.AddModelError("", "Este usuario ya tiene asignado este rol.");
                    }
                    else
                    {
                        usuarioRol.FechaAsignacion = DateTime.Now;
                        _context.Add(usuarioRol);
                        await _context.SaveChangesAsync();
                        TempData["Mensaje"] = "Rol asignado exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al asignar el rol: " + ex.Message);
                }
            }

            // Recargar las listas en caso de error
            var usuarios = _context.Usuarios
                .OrderBy(u => u.Nombre)
                .ThenBy(u => u.Apellido)
                .Select(u => new
                {
                    IdUsuario = u.IdUsuario,
                    NombreCompleto = $"{u.Nombre} {u.Apellido}"
                });

            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "NombreCompleto", usuarioRol.IdUsuario);
            ViewData["IdRol"] = new SelectList(_context.Rols.OrderBy(r => r.Nombre), "IdRol", "Nombre", usuarioRol.IdRol);

            return View(usuarioRol);
        }

        // GET: UsuarioRols/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioRol = await _context.UsuarioRols
                .Include(u => u.IdUsuarioNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.UsuarioRolId == id);

            if (usuarioRol == null)
            {
                return NotFound();
            }

            var usuarios = _context.Usuarios
                .OrderBy(u => u.Nombre)
                .ThenBy(u => u.Apellido)
                .Select(u => new
                {
                    IdUsuario = u.IdUsuario,
                    NombreCompleto = $"{u.Nombre} {u.Apellido}"
                });

            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "NombreCompleto", usuarioRol.IdUsuario);
            ViewData["IdRol"] = new SelectList(_context.Rols.OrderBy(r => r.Nombre), "IdRol", "Nombre", usuarioRol.IdRol);

            return View(usuarioRol);
        }

        // POST: UsuarioRols/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioRolId,IdUsuario,IdRol,FechaAsignacion")] UsuarioRol usuarioRol)
        {
            if (id != usuarioRol.UsuarioRolId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    // Verificar si ya existe la asignación para otro registro
                    var existeAsignacion = await _context.UsuarioRols
                        .AnyAsync(ur => ur.IdUsuario == usuarioRol.IdUsuario &&
                                      ur.IdRol == usuarioRol.IdRol &&
                                      ur.UsuarioRolId != usuarioRol.UsuarioRolId);

                    if (existeAsignacion)
                    {
                        ModelState.AddModelError("", "Este usuario ya tiene asignado este rol.");
                    }
                    else
                    {
                        _context.Update(usuarioRol);
                        await _context.SaveChangesAsync();
                        TempData["Mensaje"] = "Asignación actualizada exitosamente.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioRolExists(usuarioRol.UsuarioRolId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var usuarios = _context.Usuarios
                .OrderBy(u => u.Nombre)
                .ThenBy(u => u.Apellido)
                .Select(u => new
                {
                    IdUsuario = u.IdUsuario,
                    NombreCompleto = $"{u.Nombre} {u.Apellido}"
                });

            ViewData["IdUsuario"] = new SelectList(usuarios, "IdUsuario", "NombreCompleto", usuarioRol.IdUsuario);
            ViewData["IdRol"] = new SelectList(_context.Rols.OrderBy(r => r.Nombre), "IdRol", "Nombre", usuarioRol.IdRol);

            return View(usuarioRol);
        }

        // GET: UsuarioRols/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarioRol = await _context.UsuarioRols
                .Include(u => u.IdRolNavigation)
                .Include(u => u.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.UsuarioRolId == id);

            if (usuarioRol == null)
            {
                return NotFound();
            }

            return View(usuarioRol);
        }

        // POST: UsuarioRols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var usuarioRol = await _context.UsuarioRols
                    .Include(u => u.IdUsuarioNavigation)
                    .Include(u => u.IdRolNavigation)
                    .FirstOrDefaultAsync(u => u.UsuarioRolId == id);

                if (usuarioRol != null)
                {
                    _context.UsuarioRols.Remove(usuarioRol);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Asignación eliminada exitosamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar la asignación: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioRolExists(int id)
        {
            return _context.UsuarioRols.Any(e => e.UsuarioRolId == id);
        }
    }
}