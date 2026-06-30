using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using WebApplicationMinimalAPINetCoreTreinamento.Application.DTOs;
using WebApplicationMinimalAPINetCoreTreinamento.Application.Services;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Entities;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Events;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Exceptions;
using WebApplicationMinimalAPINetCoreTreinamento.Domain.Interfaces;

namespace WebApplicationMinimalAPINetCoreTreinamento.Application.Services
{
    public interface ICepService
    {
        Task<CepDtos> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CepDtos> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
        Task<PagedResultDto<CepDtos>> GetAllAsync(CepFilterDto filter, CancellationToken cancellationToken = default);
        Task<CepDtos> CreateAsync(CreateCepDto createDto, CancellationToken cancellationToken = default);
        Task<CepDtos> UpdateAsync(string codigo, UpdateCepDto updateDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(string codigo, CancellationToken cancellationToken = default);
        Task<CepDtos> ReactivateAsync(string codigo, CancellationToken cancellationToken = default);
        Task<IEnumerable<CepDtos>> GetByCidadeAsync(string cidade, CancellationToken cancellationToken = default);
        Task<IEnumerable<CepDtos>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default);
    }
}

    public class CepService : ICepService
    {
    private readonly ICepRepository _repository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<CepService> _logger;

    public CepService(
        ICepRepository repository,
        IMediator mediator,
        IMapper mapper,
        ILogger<CepService> logger)
    {
        _repository = repository;
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CepDtos> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cep = await _repository.GetByIdAsync(id, cancellationToken);
        if (cep == null)
            throw new EntityNotFoundException(nameof(Cep), id.ToString());

        return _mapper.Map<CepDtos>(cep);
    }

    public async Task<CepDtos> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
    {
        var normalizedCep = NormalizeCep(codigo);
        var cep = await _repository.GetByCodigoAsync(normalizedCep, cancellationToken);
        if (cep == null)
            throw new EntityNotFoundException(nameof(Cep), codigo);

        return _mapper.Map<CepDtos>(cep);
    }

    public async Task<PagedResultDto<CepDtos>> GetAllAsync(CepFilterDto filter, CancellationToken cancellationToken = default)
    {
        var ceps = await _repository.GetAllAsync(cancellationToken);

        // Apply filters
        if (!string.IsNullOrEmpty(filter.Codigo))
            ceps = ceps.Where(c => c.Codigo.Contains(filter.Codigo));
        if (!string.IsNullOrEmpty(filter.Cidade))
            ceps = ceps.Where(c => c.Cidade.Contains(filter.Cidade, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(filter.Uf))
            ceps = ceps.Where(c => c.Uf.Equals(filter.Uf, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrEmpty(filter.Bairro))
            ceps = ceps.Where(c => c.Bairro.Contains(filter.Bairro, StringComparison.OrdinalIgnoreCase));
        //if (filter.Ativo.HasValue)
        //    ceps = ceps.Where(c => c.Ativo == filter.Ativo.Value);

        var totalCount = ceps.Count();
        var pageNumber = 1;
        var pageSize = totalCount > 0 ? totalCount : 1;
        if (filter is { })
        {
            // Se o filtro tiver propriedades PageNumber e PageSize, use-as aqui.
            // Caso contrário, mantenha valores padrão.
        }
        var items = ceps
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResultDto<CepDtos>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            Items = _mapper.Map<IEnumerable<CepDtos>>(items)
        };
    }

    public async Task<CepDtos> CreateAsync(CreateCepDto createDto, CancellationToken cancellationToken = default)
    {
        var normalizedCep = NormalizeCep(createDto.Codigo);

        if (await _repository.ExistsByCodigoAsync(normalizedCep, cancellationToken))
            throw new EntityAlreadyExistsException(nameof(Cep), createDto.Codigo);

        var cep = _mapper.Map<Cep>(createDto);
        cep.SetCreatedBy("system");

        await _repository.AddAsync(cep, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // Publish domain event
        await _mediator.Publish(new CepCreatedEvent(cep.Codigo), cancellationToken);

        _logger.LogInformation($"CEP {cep.Codigo} created successfully");
        return _mapper.Map<CepDtos>(cep);
    }

    public async Task<CepDtos> UpdateAsync(string codigo, UpdateCepDto updateDto, CancellationToken cancellationToken = default)
    {
        var normalizedCep = NormalizeCep(codigo);
        var cep = await _repository.GetByCodigoAsync(normalizedCep, cancellationToken);
        if (cep == null)
            throw new EntityNotFoundException(nameof(Cep), codigo);

        _mapper.Map(updateDto, cep);
        cep.SetUpdatedBy("system");

        await _repository.UpdateAsync(cep, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // Publish domain event
        await _mediator.Publish(new CepUpdatedEvent(cep.Codigo), cancellationToken);

        _logger.LogInformation($"CEP {cep.Codigo} updated successfully");
        return _mapper.Map<CepDtos>(cep);
    }

    public async Task DeleteAsync(string codigo, CancellationToken cancellationToken = default)
    {
        var normalizedCep = NormalizeCep(codigo);
        var cep = await _repository.GetByCodigoAsync(normalizedCep, cancellationToken);
        if (cep == null)
            throw new EntityNotFoundException(nameof(Cep), codigo);

        cep.Desativar();
        cep.SetUpdatedBy("system");

        await _repository.UpdateAsync(cep, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // Publish domain event
        await _mediator.Publish(new CepDeletedEvent(cep.Codigo), cancellationToken);

        _logger.LogInformation($"CEP {cep.Codigo} deleted successfully");
    }

    public async Task<CepDtos> ReactivateAsync(string codigo, CancellationToken cancellationToken = default)
    {
        var normalizedCep = NormalizeCep(codigo);
        var cep = await _repository.GetByCodigoAsync(normalizedCep, cancellationToken);
        if (cep == null)
            throw new EntityNotFoundException(nameof(Cep), codigo);

        cep.Ativar();
        cep.SetUpdatedBy("system");

        await _repository.UpdateAsync(cep, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        // Publish domain event
        await _mediator.Publish(new CepReactivatedEvent(cep.Codigo), cancellationToken);

        _logger.LogInformation($"CEP {cep.Codigo} reactivated successfully");
        return _mapper.Map<CepDtos>(cep);
    }

    public async Task<IEnumerable<CepDtos>> GetByCidadeAsync(string cidade, CancellationToken cancellationToken = default)
    {
        var ceps = await _repository.GetByCidadeAsync(cidade, cancellationToken);
        return _mapper.Map<IEnumerable<CepDtos>>(ceps);
    }

    public async Task<IEnumerable<CepDtos>> GetByEstadoAsync(string estado, CancellationToken cancellationToken = default)
    {
        var ceps = await _repository.GetByEstadoAsync(estado, cancellationToken);
        return _mapper.Map<IEnumerable<CepDtos>>(ceps);
    }

    private static string NormalizeCep(string cep)
    {
        return new string(cep.Where(char.IsDigit).ToArray());
    }
}
}
