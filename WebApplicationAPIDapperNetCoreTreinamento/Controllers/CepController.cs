using Microsoft.AspNetCore.Mvc;
using WebApplicationAPIDapperNetCoreTreinamento.DTOs;
using WebApplicationAPIDapperNetCoreTreinamento.Services;

namespace WebApplicationAPIDapperNetCoreTreinamento.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CepController:ControllerBase 
    {
        private readonly ICepService _cepService;

        public CepController(ICepService cepService)
        {
            _cepService = cepService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CepResponseDto>>> GetAll()
        {
            var ceps = await _cepService.GetAllAsync();
            return Ok(ceps);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CepResponseDto>> GetById(int id)
        {
            var cep = await _cepService.GetByIdAsync(id);
            if (cep == null)
                return NotFound($"CEP com ID {id} não encontrado");

            return Ok(cep);
        }

        [HttpGet("buscar/{cep}")]
        public async Task<ActionResult<CepResponseDto>> GetByCep(string cep)
        {
            var cepEntity = await _cepService.GetByCepAsync(cep);
            if (cepEntity == null)
                return NotFound($"CEP {cep} não encontrado");

            return Ok(cepEntity);
        }

        [HttpPost]
        public async Task<ActionResult<CepResponseDto>> Create([FromBody] CepDto cepDto)
        {
            try
            {
                var cep = await _cepService.CreateAsync(cepDto);
                return CreatedAtAction(nameof(GetById), new { id = cep.Id }, cep);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CepDto cepDto)
        {
            try
            {
                var updated = await _cepService.UpdateAsync(id, cepDto);
                if (!updated)
                    return NotFound($"CEP com ID {id} não encontrado");

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _cepService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"CEP com ID {id} não encontrado");

            return NoContent();
        }
    }
}
