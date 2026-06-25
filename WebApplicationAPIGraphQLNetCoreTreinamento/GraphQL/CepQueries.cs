using WebApplicationAPIGraphQLNetCoreTreinamento.Models;
using WebApplicationAPIGraphQLNetCoreTreinamento.Repositories;

namespace WebApplicationAPIGraphQLNetCoreTreinamento.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class CepQueries
    {
        public async Task<IEnumerable<Cep>> GetCeps([Service] ICepRepository repository)
        {
            return await repository.GetAllAsync();
        }

        public async Task<Cep?> GetCepById([Service] ICepRepository repository, int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task<Cep?> GetCepByCodigo([Service] ICepRepository repository, string codigo)
        {
            return await repository.GetByCodigoAsync(codigo);
        }
    }
}
