using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using WebApplicationAPINetCoreTreinamento.Data;
using WebApplicationAPINetCoreTreinamento.Models;

namespace CepApi.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CepController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CepController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/cep
        [HttpGet]
        [MapToApiVersion("1.0")]// [MapToApiVersion("2.0")] // opcional, pode ser só na v1
        [SwaggerOperation(Summary = "Lista todos os CEPs")]
        public async Task<ActionResult<IEnumerable<Cep>>> GetCep()
        {
            return await _context.Cep.ToListAsync();
        }

        // GET: api/cep versionada
        [HttpGet]
        [MapToApiVersion("2.0")]// pode ser só na v2
        public ActionResult<IEnumerable<Cep>> GetV2()
        {
            return new Cep[]
            {
                new Cep { Id = 1, Codigo = "01001-000", Cidade = "São Paulo", Uf = "SP" },
                new Cep { Id = 2, Codigo = "02002-000", Cidade = "São Paulo", Uf = "SP" }
            };
        }

        // GET: api/cep/5
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]// [MapToApiVersion("2.0")] // opcional, pode ser só na v1
        [SwaggerOperation(Summary = "Busca um CEP por Id")]
        public async Task<ActionResult<Cep>> GetCep(int id)
        {
            var cep = await _context.Cep.FindAsync(id);

            if (cep == null)
                return NotFound();

            return cep;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]// [MapToApiVersion("2.0")] // opcional, pode ser só na v1
        [SwaggerOperation(Summary = "Cria um novo CEP")]
        public async Task<ActionResult<Cep>> Create(Cep cep)
        {
            _context.Cep.Add(cep);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCep), new { id = cep.Id }, cep);
        }

        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]// [MapToApiVersion("2.0")] // opcional, pode ser só na v1
        [SwaggerOperation(Summary = "Atualiza um CEP existente")]
        public async Task<IActionResult> Update(int id, Cep cep)
        {
            if (id != cep.Id) return BadRequest();

            _context.Entry(cep).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")] // [MapToApiVersion("2.0")] // opcional, pode ser só na v1
        [SwaggerOperation(Summary = "Remove um CEP")]
        public async Task<IActionResult> Delete(int id)
        {
            var cep = await _context.Cep.FindAsync(id);
            if (cep is null) return NotFound();

            _context.Cep.Remove(cep);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
