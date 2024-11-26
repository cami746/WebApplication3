using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class RegistroController : Controller
    {
        private readonly PlSubastas2Context _context;

        public RegistroController(PlSubastas2Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Cargar los roles desde la base de datos
            ViewBag.Roles = _context.Rols.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarUsuario(Usuario usuario, int rolId)
        {
            if (ModelState.IsValid)
            {
                // Agregar el nuevo usuario
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Asignar rol al usuario
                var usuarioRol = new UsuarioRol
                {
                    IdUsuario = usuario.IdUsuario, // Asegúrate de que IdUsuario esté configurado correctamente
                    IdRol = rolId,
                    FechaAsignacion = DateTime.Now
                };
                _context.UsuarioRols.Add(usuarioRol);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Login"); // Redirigir a la página de inicio de sesión
            }

            // Si hay un error, volver a cargar los roles y mostrar la vista
            ViewBag.Roles = _context.Rols.ToList();
            return View("Index", usuario);
        }
    }
}
