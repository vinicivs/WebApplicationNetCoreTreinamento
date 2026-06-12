using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplicationRazorNetCoreTreinamento.Models;
using WebApplicationRazorNetCoreTreinamento.Data;

namespace WebApplicationRazorNetCoreTreinamento.Pages.CepPages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Cep> Cep { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Cep = await _context.Cep.ToListAsync();
    }
}
