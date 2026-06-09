
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAPINetCoreTreinamento.Models;
using WebApplicationAPINetCoreTreinamento.Data;

public class CepsController : Controller
{
    private readonly AppDbContext _context;

    public CepsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: CEPSS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Ceps.ToListAsync());
    }

    // GET: CEPSS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ceps = await _context.Ceps
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ceps == null)
        {
            return NotFound();
        }

        return View(ceps);
    }

    // GET: CEPSS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CEPSS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Uf")] Ceps ceps)
    {
        if (ModelState.IsValid)
        {
            _context.Add(ceps);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(ceps);
    }

    // GET: CEPSS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ceps = await _context.Ceps.FindAsync(id);
        if (ceps == null)
        {
            return NotFound();
        }
        return View(ceps);
    }

    // POST: CEPSS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Uf")] Ceps ceps)
    {
        if (id != ceps.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ceps);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CepsExists(ceps.Id))
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
        return View(ceps);
    }

    // GET: CEPSS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var ceps = await _context.Ceps
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ceps == null)
        {
            return NotFound();
        }

        return View(ceps);
    }

    // POST: CEPSS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var ceps = await _context.Ceps.FindAsync(id);
        if (ceps != null)
        {
            _context.Ceps.Remove(ceps);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CepsExists(int? id)
    {
        return _context.Ceps.Any(e => e.Id == id);
    }
}
