using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using Bogus;
using WebApplicationAPIMinimalNetCoreTreinamento.Models;
using WebApplicationAPIMinimalNetCoreTreinamento.DTOs;

namespace TestProjectWebApplicationAPIMinimal.Helpers
{
    public class TestDataFactory
    {
        public static Faker<Cep> CreateCepFaker()
        {
            return new Faker<Cep>("pt_BR")
                .RuleFor(c => c.Id, f => f.Random.Int(1, 9999))
                .RuleFor(c => c.Codigo, f => f.Address.ZipCode())
                .RuleFor(c => c.Logradouro, f => f.Address.StreetName())
                .RuleFor(c => c.Numero, f => f.Random.Number(1, 9999).ToString())
                .RuleFor(c => c.Complemento, f => f.Lorem.Sentence(3))
                .RuleFor(c => c.Bairro, f => f.Address.County())
                .RuleFor(c => c.Cidade, f => f.Address.City())
                .RuleFor(c => c.Uf, f => f.Address.StateAbbr());
        }

        public static Cep CreateValidCep()
        {
            return CreateCepFaker().Generate();
        }

        public static List<Cep> CreateCeps(int count)
        {
            return CreateCepFaker().Generate(count);
        }

        public static CepCreateDto CreateValidCepCreateDto()
        {
            var faker = new Faker("pt_BR");
            return new CepCreateDto
            {
                Codigo = faker.Address.ZipCode(),
                Logradouro = faker.Address.StreetName(),
                Numero = faker.Random.Number(1, 9999).ToString(),
                Complemento = faker.Lorem.Sentence(3),
                Bairro = faker.Address.County(),
                Cidade = faker.Address.City(),
                Uf = faker.Address.StateAbbr()
            };
        }

        public static CepUpdateDto CreateValidCepUpdateDto()
        {
            var faker = new Faker("pt_BR");
            return new CepUpdateDto
            {
                Codigo = faker.Address.ZipCode(),
                Logradouro = faker.Address.StreetName(),
                Numero = faker.Random.Number(1, 9999).ToString(),
                Complemento = faker.Lorem.Sentence(3),
                Bairro = faker.Address.County(),
                Cidade = faker.Address.City(),
                Uf = faker.Address.StateAbbr()
            };
        }

        public static CepDto CreateValidCepDto(Cep cep)
        {
            return new CepDto
            {
                Id = cep.Id,
                Codigo = cep.Codigo,
                Logradouro = cep.Logradouro,
                Numero = cep.Numero,
                Complemento = cep.Complemento,
                Bairro = cep.Bairro,
                Cidade = cep.Cidade,
                Uf = cep.Uf
            };
        }
    }
}
