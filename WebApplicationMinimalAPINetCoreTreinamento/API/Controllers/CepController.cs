using Microsoft.AspNetCore.Mvc;
using WebApplicationMinimalAPINetCoreTreinamento.Application.DTOs;
using WebApplicationMinimalAPINetCoreTreinamento.Application.Services;

namespace WebApplicationMinimalAPINetCoreTreinamento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CepController : ControllerBase
    {
        private readonly ICepService _cepService;
        private readonly ILogger<CepController> _logger;

        public CepController(ICepService cepService, ILogger<CepController> logger)
        {
            _cepService = cepService;
            _logger = logger;
        }

        [HttpGet("{codigo}")]
        [ProducesResponseType(typeof(CepDtos), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 8)
                return BadRequest("CEP deve conter 8 dígitos");

            var cep = await _cepService.GetByCodigoAsync(codigo);
            return Ok(cep);
        }

        [HttpGet("by-id/{id:guid}")]
        [ProducesResponseType(typeof(CepDtos), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cep = await _cepService.GetByIdAsync(id);
            return Ok(cep);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<CepDtos>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] CepFilterDto filter)
        {
            var result = await _cepService.GetAllAsync(filter);
            return Ok(result);
        }

        [HttpGet("cidade/{cidade}")]
        [ProducesResponseType(typeof(IEnumerable<CepDtos>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCidade(string cidade)
        {
            var ceps = await _cepService.GetByCidadeAsync(cidade);
            return Ok(ceps);
        }

        [HttpGet("estado/{estado}")]
        [ProducesResponseType(typeof(IEnumerable<CepDtos>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByEstado(string estado)
        {
            var ceps = await _cepService.GetByEstadoAsync(estado);
            return Ok(ceps);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CepDtos), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateCepDto createDto)
        {
            var cep = await _cepService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetByCodigo), new { codigo = cep.Codigo }, cep);
        }

        [HttpPut("{codigo}")]
        [ProducesResponseType(typeof(CepDtos), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string codigo, [FromBody] UpdateCepDto updateDto)
        {
            if (string.IsNullOrWhiteSpace(codigo) || codigo.Length != 8)
                return BadRequest("CEP deve conter 8 dígitos");

            var cep = await _cepService.UpdateAsync(codigo, updateDto);
            return Ok(cep);
        }

        [HttpPatch("{codigo}/reactivate")]
        [ProducesResponseType(typeof(CepDtos), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Reactivate(string codigo)
        {
            var cep = await _cepService.ReactivateAsync(codigo);
            return Ok(cep);
        }

        [HttpDelete("{codigo}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string codigo)
        {
            await _cepService.DeleteAsync(codigo);
            return NoContent();
        }
    }
}
