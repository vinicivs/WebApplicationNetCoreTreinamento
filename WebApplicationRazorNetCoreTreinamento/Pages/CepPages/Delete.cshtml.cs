using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplicationRazorNetCoreTreinamento.Models;
using WebApplicationRazorNetCoreTreinamento.Data;

namespace WebApplicationRazorNetCoreTreinamento.Pages.CepPages;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Cep Cep { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var cep = await _context.Cep.FirstOrDefaultAsync(m => m.Id == id);
        if (cep is null)
        {
            return NotFound();
        }
        else
        {
            Cep = cep;
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var cep = await _context.Cep.FindAsync(id);
        if (cep != null)
        {
            Cep = cep;
            _context.Cep.Remove(Cep);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
