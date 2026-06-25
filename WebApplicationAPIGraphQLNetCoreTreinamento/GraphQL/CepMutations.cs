using WebApplicationAPIGraphQLNetCoreTreinamento.Dtos;
using WebApplicationAPIGraphQLNetCoreTreinamento.Models;
using WebApplicationAPIGraphQLNetCoreTreinamento.Repositories;

namespace WebApplicationAPIGraphQLNetCoreTreinamento.GraphQL
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class CepMutations
    {
        public async Task<Cep> CreateCep(
            [Service] ICepRepository repository,
            CepInput input)
        {
            var cep = new Cep
            {
                Codigo = input.Codigo,
                Logradouro = input.Logradouro,
                Numero = input.Numero,
                Complemento = input.Complemento,
                Bairro = input.Bairro,
                Cidade = input.Cidade,
                Uf = input.Uf
            };

            return await repository.CreateAsync(cep);
        }

        public async Task<Cep> UpdateCep(
            [Service] ICepRepository repository,
            CepUpdateInput input)
        {
            var cep = await repository.GetByIdAsync(input.Id);
            if (cep == null)
                throw new GraphQLException($"CEP com ID {input.Id} não encontrado");

            cep.Codigo = input.Codigo;
            cep.Logradouro = input.Logradouro;
            cep.Numero = input.Numero;
            cep.Complemento = input.Complemento;
            cep.Bairro = input.Bairro;
            cep.Cidade = input.Cidade;
            cep.Uf = input.Uf;
            return await repository.UpdateAsync(cep);
        }

        public async Task<bool> DeleteCep(
            [Service] ICepRepository repository,
            int id)
        {
            return await repository.DeleteAsync(id);
        }
    }
}
