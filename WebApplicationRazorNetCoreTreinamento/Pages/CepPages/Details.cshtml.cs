using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplicationRazorNetCoreTreinamento.Models;
using WebApplicationRazorNetCoreTreinamento.Data;

namespace WebApplicationRazorNetCoreTreinamento.Pages.CepPages;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;
    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

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
}
