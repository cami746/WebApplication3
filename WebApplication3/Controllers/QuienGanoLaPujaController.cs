using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class QuienGanoLaPujaController : Controller
    {
        private readonly PlSubastas2Context _context;

        public QuienGanoLaPujaController(PlSubastas2Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Obtener los ganadores agrupando por SubastaId y UsuarioId
            var ganadoresTemp = await _context.Pujas
                .Join(_context.Inscripcions,
                      p => p.InscripcionId,
                      i => i.InscripcionId,
                      (p, i) => new { p, i, i.Subasta })
                .GroupBy(x => new { x.i.SubastaId, x.i.UsuarioId })
                .Select(g => new
                {
                    SubastaId = g.Key.SubastaId,
                    SubastaTitulo = g.First().Subasta.Titulo,
                    UsuarioId = g.Key.UsuarioId,
                    MontoTotal = g.Sum(e => e.p.Monto),
                    UltimaFechaPuja = g.Max(e => e.p.FechaPuja)
                })
                .OrderByDescending(g => g.MontoTotal)
                .GroupBy(g => g.SubastaId)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();

            // Ahora que tenemos el resultado en memoria, hacemos la unión con los usuarios
            var ganadores = ganadoresTemp.Join(_context.Usuarios,
                      g => g.UsuarioId,
                      u => u.IdUsuario,
                      (g, u) => new Ganador
                      {
                          SubastaId = g.SubastaId,
                          SubastaTitulo = g.SubastaTitulo,
                          UsuarioNombre = u.Nombre + " " + u.Apellido,
                          MontoTotal = g.MontoTotal,
                          UltimaFechaPuja = g.UltimaFechaPuja
                      }).ToList();

            return View(ganadores);
        }
    }
}