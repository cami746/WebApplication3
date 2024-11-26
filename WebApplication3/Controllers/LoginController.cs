using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using Microsoft.EntityFrameworkCore; // Añadir esta línea

namespace WebApplication3.Controllers
{
    public class LoginController : Controller
    {
        private readonly PlSubastas2Context _context;
        public LoginController(PlSubastas2Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Inicializa el modelo Usuario para evitar null reference exception
            var usuario = new Usuario();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IniciarSesion(string email, string contrasena)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasena))
            {
                ViewBag.ErrorMessage = "El correo electrónico y la contraseña son obligatorios.";
                // Retorna el modelo vacío para que se muestre en la vista
                var usuario = new Usuario { Email = email }; // Conservar el correo ingresado
                return View("Index", usuario);
            }

            var usuarioDb = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuarioDb == null || !VerificarContrasena(contrasena, usuarioDb.Contrasena))
            {
                ViewBag.ErrorMessage = "Correo electrónico o contraseña inválidos.";
                // Retorna el modelo vacío para que se muestre en la vista
                var usuario = new Usuario { Email = email }; // Conservar el correo ingresado
                return View("Index", usuario);
            }

            // Actualizar el último acceso
            usuarioDb.UltimoAcceso = DateTime.Now;
            await _context.SaveChangesAsync();

            // Redirigir a la página principal después de iniciar sesión exitosamente
            return RedirectToAction("Index", "Home");
        }

        private bool VerificarContrasena(string contrasenaIngresada, string contrasenaAlmacenada)
        {
            // Comparación simple para propósitos de demostración
            return contrasenaIngresada == contrasenaAlmacenada;
        }
    }
}