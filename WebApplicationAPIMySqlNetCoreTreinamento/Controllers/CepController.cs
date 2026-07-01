using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPIMySqlNetCoreTreinamento.Data;

namespace WebApplicationAPIMySqlNetCoreTreinamento.Controllers
{
    public class CepController : Controller
    {
        private readonly AppDbContext _context;

        public CepController(AppDbContext context)
        {
            _context = context;
        }

        // Listar todos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ceps.ToListAsync());
        }

        // Detalhes
        public async Task<IActionResult> Details(int id)
        {
            var cep = await _context.Ceps.FindAsync(id);
            if (cep == null) return NotFound();
            return View(cep);
        }

        // Criar
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Cep cep)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cep);
        }

        // Editar
        public async Task<IActionResult> Edit(int id)
        {
            var cep = await _context.Ceps.FindAsync(id);
            if (cep == null) return NotFound();
            return View(cep);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Cep cep)
        {
            if (id != cep.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(cep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cep);
        }

        // Excluir
        public async Task<IActionResult> Delete(int id)
        {
            var cep = await _context.Ceps.FindAsync(id);
            if (cep == null) return NotFound();
            return View(cep);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cep = await _context.Ceps.FindAsync(id);
            _context.Ceps.Remove(cep);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
