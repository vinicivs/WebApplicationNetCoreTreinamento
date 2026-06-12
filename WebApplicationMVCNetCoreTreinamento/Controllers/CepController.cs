using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVCNetCoreTreinamento.Models;
using WebApplicationMVCNetCoreTreinamento.Data;


    public class CepController : Controller
    {
        private readonly AppDbContext _context;

        public CepController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CEPS
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cep.ToListAsync());
        }

        // GET: CEPS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cep = await _context.Cep
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cep == null)
            {
                return NotFound();
            }

            return View(cep);
        }

        // GET: CEPS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CEPS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Logradouro,Numero,Complemento,Bairro,Cidade,Uf")] Cep cep)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cep);
        }

        // GET: CEPS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cep = await _context.Cep.FindAsync(id);
            if (cep == null)
            {
                return NotFound();
            }
            return View(cep);
        }

        // POST: CEPS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Codigo,Logradouro,Numero,Complemento,Bairro,Cidade,Uf")] Cep cep)
        {
            if (id != cep.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CepExists(cep.Id))
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
            return View(cep);
        }

        // GET: CEPS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cep = await _context.Cep
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cep == null)
            {
                return NotFound();
            }

            return View(cep);
        }

        // POST: CEPS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var cep = await _context.Cep.FindAsync(id);
            if (cep != null)
            {
                _context.Cep.Remove(cep);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CepExists(int? id)
        {
            return _context.Cep.Any(e => e.Id == id);
        }
    }

