using Moq;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using WebApplicationAPIDapperNetCoreTreinamento.Models;
using WebApplicationAPIDapperNetCoreTreinamento.Repositories;
using WebApplicationAPIDapperNetCoreTreinamento.Data;

namespace TestProjectWebApplicationAPIDapperNetCoreTreinamento.UnitTests.Repositories
{
    public class CepRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _sqliteConnection;
        private readonly ICepRepository _repository;
        private readonly Mock<DatabaseContext> _mockContext;

        public CepRepositoryTests()
        {
            // Criar conexão SQLite em memória
            _sqliteConnection = new SqliteConnection("Data Source=:memory:");
            _sqliteConnection.Open();

            // Criar a tabela Cep
            using (var command = _sqliteConnection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE Cep (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Codigo TEXT NOT NULL,
                        Logradouro TEXT NOT NULL,
                        Numero TEXT,
                        Complemento TEXT,
                        Bairro TEXT NOT NULL,
                        Cidade TEXT NOT NULL,
                        Uf TEXT NOT NULL
                    )";
                command.ExecuteNonQuery();
            }

            // Inserir dados de teste
            using (var command = _sqliteConnection.CreateCommand())
            {
                command.CommandText = @"
                    INSERT INTO Cep (Codigo, Logradouro, Numero, Complemento, Bairro, Cidade, Uf)
                    VALUES ('01001-000', 'Praça da Sé', '123', 'COMPL', 'Bairro', 'São Paulo', 'SP'),
                           ('20040-002', 'Av. Presidente Vargas', '124', 'COMPL1', 'Bairro', 'Rio de Janeiro', 'RJ')";
                command.ExecuteNonQuery();
            }

            // Mock do DatabaseContext para retornar a conexão SQLite
            _mockContext = new Mock<DatabaseContext>(MockBehavior.Loose);
            _mockContext.Setup(x => x.CreateConnection())
                       .Returns(_sqliteConnection);

            _repository = new CepRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCeps()
        {
            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Codigo.Should().Be("01001-000");
            result.Last().Codigo.Should().Be("20040-002");
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnCep()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Codigo.Should().Be("01001-000");
            result.Logradouro.Should().Be("Praça da Sé");
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByCepAsync_WithValidCep_ShouldReturnCep()
        {
            // Act
            var result = await _repository.GetByCepAsync("01001-000");

            // Assert
            result.Should().NotBeNull();
            result!.Codigo.Should().Be("01001-000");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNewId()
        {
            // Arrange
            var newCep = new Cep
            {
                Codigo = "12345-678",
                Logradouro = "Rua Teste",
                Numero = null,
                Complemento = null,
                Bairro = "Centro",
                Cidade = "São Paulo",
                Uf = "SP"
            };

            // Act
            var result = await _repository.CreateAsync(newCep);

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            var cepToUpdate = new Cep
            {
                Id = 1,
                Codigo = "01001-000",
                Logradouro = "Praça da Sé Atualizada",
                Numero = null,
                Complemento = null,
                Bairro = "Sé",
                Cidade = "São Paulo",
                Uf = "SP"
            };

            // Act
            var result = await _repository.UpdateAsync(cepToUpdate);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldReturnTrue()
        {
            // Act
            var result = await _repository.DeleteAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.DeleteAsync(999);

            // Assert
            result.Should().BeFalse();
        }

        public void Dispose()
        {
            _sqliteConnection?.Dispose();
        }
    }
}
