using WebApplicationAPIDapperNetCoreTreinamento.DTOs;
using WebApplicationAPIDapperNetCoreTreinamento.Models;
using WebApplicationAPIDapperNetCoreTreinamento.Repositories;

namespace WebApplicationAPIDapperNetCoreTreinamento.Services
{
    public interface ICepService
    {
        Task<IEnumerable<CepResponseDto>> GetAllAsync();
        Task<CepResponseDto?> GetByIdAsync(int id);
        Task<CepResponseDto?> GetByCepAsync(string cep);
        Task<CepResponseDto> CreateAsync(CepDto cepDto);
        Task<bool> UpdateAsync(int id, CepDto cepDto);
        Task<bool> DeleteAsync(int id);
    }

    public class CepService : ICepService
    {
        private readonly ICepRepository _repository;

        public CepService(ICepRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CepResponseDto>> GetAllAsync()
        {
            var ceps = await _repository.GetAllAsync();
            return ceps.Select(MapToResponseDto);
        }

        public async Task<CepResponseDto?> GetByIdAsync(int id)
        {
            var cep = await _repository.GetByIdAsync(id);
            return cep == null ? null : MapToResponseDto(cep);
        }

        public async Task<CepResponseDto?> GetByCepAsync(string cep)
        {
            var cepEntity = await _repository.GetByCepAsync(cep);
            return cepEntity == null ? null : MapToResponseDto(cepEntity);
        }

        public async Task<CepResponseDto> CreateAsync(CepDto cepDto)
        {
            // Verifica se o CEP já existe
            var existingCep = await _repository.GetByCepAsync(cepDto.Codigo);
            if (existingCep != null)
                throw new InvalidOperationException("CEP já cadastrado");

            var cep = new Cep
            {
                Codigo = cepDto.Codigo,
                Logradouro = cepDto.Logradouro,
                Numero = cepDto.Numero,
                Complemento = cepDto.Complemento,
                Bairro = cepDto.Bairro,
                Cidade = cepDto.Cidade,
                Uf = cepDto.Uf
            };

            var id = await _repository.CreateAsync(cep);
            cep.Id = id;

            return MapToResponseDto(cep);
        }

        public async Task<bool> UpdateAsync(int id, CepDto cepDto)
        {
            var existingCep = await _repository.GetByIdAsync(id);
            if (existingCep == null)
                return false;

            // Verifica se o novo CEP já existe (se for diferente)
            if (existingCep.Codigo != cepDto.Codigo)
            {
                var cepExists = await _repository.GetByCepAsync(cepDto.Codigo);
                if (cepExists != null)
                    throw new InvalidOperationException("CEP já cadastrado para outro endereço");
            }

            existingCep.Codigo = cepDto.Codigo;
            existingCep.Logradouro = cepDto.Logradouro;
            existingCep.Numero = cepDto.Numero;
            existingCep.Complemento = cepDto.Complemento;
            existingCep.Bairro = cepDto.Bairro;
            existingCep.Cidade = cepDto.Cidade;
            existingCep.Uf = cepDto.Uf;

            return await _repository.UpdateAsync(existingCep);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static CepResponseDto MapToResponseDto(Cep cep)
        {
            return new CepResponseDto
            {
                Id = cep.Id,
                Codigo = cep.Codigo,
                Logradouro = cep.Logradouro,
                Numero = cep.Numero,
                Complemento = cep.Complemento,
                Bairro = cep.Bairro,
                Cidade = cep.Cidade,
                Uf = cep.Uf,
                
            };
        }
    }
}
