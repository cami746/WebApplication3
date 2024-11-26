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
    public class TipoDeSubastumsController : Controller
    {
        private readonly PlSubastas2Context _context;

        public TipoDeSubastumsController(PlSubastas2Context context)
        {
            _context = context;
        }

        // GET: TipoDeSubastums
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoDeSubasta.ToListAsync());
        }

        // GET: TipoDeSubastums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeSubastum = await _context.TipoDeSubasta
                .FirstOrDefaultAsync(m => m.TipoDeSubastaId == id);
            if (tipoDeSubastum == null)
            {
                return NotFound();
            }

            return View(tipoDeSubastum);
        }

        // GET: TipoDeSubastums/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDeSubastums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoDeSubastaId,Nombre")] TipoDeSubastum tipoDeSubastum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoDeSubastum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDeSubastum);
        }

        // GET: TipoDeSubastums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeSubastum = await _context.TipoDeSubasta.FindAsync(id);
            if (tipoDeSubastum == null)
            {
                return NotFound();
            }
            return View(tipoDeSubastum);
        }

        // POST: TipoDeSubastums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TipoDeSubastaId,Nombre")] TipoDeSubastum tipoDeSubastum)
        {
            if (id != tipoDeSubastum.TipoDeSubastaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoDeSubastum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoDeSubastumExists(tipoDeSubastum.TipoDeSubastaId))
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
            return View(tipoDeSubastum);
        }

        // GET: TipoDeSubastums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeSubastum = await _context.TipoDeSubasta
                .FirstOrDefaultAsync(m => m.TipoDeSubastaId == id);
            if (tipoDeSubastum == null)
            {
                return NotFound();
            }

            return View(tipoDeSubastum);
        }

        // POST: TipoDeSubastums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoDeSubastum = await _context.TipoDeSubasta.FindAsync(id);
            if (tipoDeSubastum != null)
            {
                _context.TipoDeSubasta.Remove(tipoDeSubastum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoDeSubastumExists(int id)
        {
            return _context.TipoDeSubasta.Any(e => e.TipoDeSubastaId == id);
        }
    }
}
