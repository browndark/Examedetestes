using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace MinhasFinancas.Tests.Integration;

public class PessoaPersistenceTests : IAsyncLifetime
{
    private MinhasFinancasDbContext _context;

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<MinhasFinancasDbContext>()
            .UseSqlite("Data Source=:memory:")
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine)
            .Options;

        _context = new MinhasFinancasDbContext(options);
        
        // Abrir conexão
        await _context.Database.OpenConnectionAsync();
        
        // Criar as tabelas
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }

    [Fact]
    public async Task ShouldPersistAndRetrievePessoa()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "João Silva",
            DataNascimento = new DateTime(1990, 5, 15)
        };

        // Act
        _context.Set<Pessoa>().Add(pessoa);
        await _context.SaveChangesAsync();

        var retrieved = await _context.Set<Pessoa>().FirstOrDefaultAsync(p => p.Id == pessoa.Id);

        // Assert
        retrieved.Should().NotBeNull();
        retrieved!.Nome.Should().Be("João Silva");
        retrieved!.Idade.Should().BeGreaterThan(30);
    }

    [Fact]
    public async Task ShouldUpdatePessoa()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Ana Costa",
            DataNascimento = DateTime.Today.AddYears(-25)
        };

        _context.Set<Pessoa>().Add(pessoa);
        await _context.SaveChangesAsync();

        // Act
        pessoa.Nome = "Ana Silva Costa";
        _context.Set<Pessoa>().Update(pessoa);
        await _context.SaveChangesAsync();

        var updated = await _context.Set<Pessoa>().FirstOrDefaultAsync(p => p.Id == pessoa.Id);

        // Assert
        updated.Should().NotBeNull();
        updated!.Nome.Should().Be("Ana Silva Costa");
    }

    [Fact]
    public async Task ShouldDeletePessoa()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Carlos Mendes",
            DataNascimento = DateTime.Today.AddYears(-30)
        };

        _context.Set<Pessoa>().Add(pessoa);
        await _context.SaveChangesAsync();

        var pessoaId = pessoa.Id;

        // Act
        _context.Set<Pessoa>().Remove(pessoa);
        await _context.SaveChangesAsync();

        var deleted = await _context.Set<Pessoa>().FirstOrDefaultAsync(p => p.Id == pessoaId);

        // Assert
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task ShouldCalculaIdadeCorrectlyAfterPersistence()
    {
        // Arrange
        var dataNasc = DateTime.Today.AddYears(-20).AddDays(-15);
        var pessoa = new Pessoa
        {
            Nome = "Gabriel Junior",
            DataNascimento = dataNasc
        };

        _context.Set<Pessoa>().Add(pessoa);
        await _context.SaveChangesAsync();

        // Act
        var retrieved = await _context.Set<Pessoa>().FirstOrDefaultAsync(p => p.Id == pessoa.Id);

        // Assert
        retrieved.Should().NotBeNull();
        retrieved!.Idade.Should().Be(20);
    }
}
