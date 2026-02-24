using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Infrastructure.Data;
using MinhasFinancas.Infrastructure.Repositories;
using MinhasFinancas.Infrastructure;
using System;
using System.Threading.Tasks;

namespace MinhasFinancas.Tests.Integration;

public class TransacaoBusinnessRulesIntegrationTests : IAsyncLifetime
{
    private MinhasFinancasDbContext _context;
    private UnitOfWork _unitOfWork;
    private TransacaoService _transacaoService;

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

        _unitOfWork = new UnitOfWork(_context);
        _transacaoService = new TransacaoService(_unitOfWork);
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
    public async Task ShouldNotAllowMinorToCreateReceita()
    {
        // Arrange - Create a minor person
        var menorDeIdade = new Pessoa
        {
            Nome = "Pedro Junior",
            DataNascimento = DateTime.Today.AddYears(-15)
        };

        await _unitOfWork.Pessoas.AddAsync(menorDeIdade);

        // Create a receita category
        var categoriaReceita = new Categoria
        {
            Descricao = "Mesada",
            Finalidade = Categoria.EFinalidade.Receita
        };

        await _unitOfWork.Categorias.AddAsync(categoriaReceita);
        await _unitOfWork.SaveChangesAsync();

        var dto = new CreateTransacaoDto
        {
            Descricao = "Mesada do mês",
            Valor = 50m,
            Tipo = Transacao.ETipo.Receita,
            Data = DateTime.Today,
            CategoriaId = categoriaReceita.Id,
            PessoaId = menorDeIdade.Id
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _transacaoService.CreateAsync(dto));

        exception.Message.Should().Contain("Menores de 18 anos não podem registrar receitas");
    }

    [Fact]
    public async Task ShouldAllowMinorToCreateDespesa()
    {
        // Arrange
        var menorDeIdade = new Pessoa
        {
            Nome = "Maria Jovem",
            DataNascimento = DateTime.Today.AddYears(-14)
        };

        await _unitOfWork.Pessoas.AddAsync(menorDeIdade);

        var categoriaDespesa = new Categoria
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        await _unitOfWork.Categorias.AddAsync(categoriaDespesa);
        await _unitOfWork.SaveChangesAsync();

        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra de doces",
            Valor = 10m,
            Tipo = Transacao.ETipo.Despesa,
            Data = DateTime.Today,
            CategoriaId = categoriaDespesa.Id,
            PessoaId = menorDeIdade.Id
        };

        // Act
        var result = await _transacaoService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Descricao.Should().Be("Compra de doces");
    }

    [Fact]
    public async Task ShouldEnforceCategoryFinality()
    {
        // Arrange - Create a despesa category
        var categoriaDespesa = new Categoria
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        await _unitOfWork.Categorias.AddAsync(categoriaDespesa);

        var pessoa = new Pessoa
        {
            Nome = "Roberto",
            DataNascimento = DateTime.Today.AddYears(-35)
        };

        await _unitOfWork.Pessoas.AddAsync(pessoa);
        await _unitOfWork.SaveChangesAsync();

        var dto = new CreateTransacaoDto
        {
            Descricao = "Ganho inesperado",
            Valor = 500m,
            Tipo = Transacao.ETipo.Receita,  // Trying to create receita in despesa category
            Data = DateTime.Today,
            CategoriaId = categoriaDespesa.Id,
            PessoaId = pessoa.Id
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _transacaoService.CreateAsync(dto));

        exception.Message.Should().Contain("receita em categoria de despesa");
    }

    [Fact]
    public async Task ShouldCreateTransacaoWithAmbasCategory()
    {
        // Arrange - Create an "Ambas" category
        var categoriaAmbas = new Categoria
        {
            Descricao = "Investimentos",
            Finalidade = Categoria.EFinalidade.Ambas
        };

        await _unitOfWork.Categorias.AddAsync(categoriaAmbas);

        var pessoa = new Pessoa
        {
            Nome = "Maria Invest",
            DataNascimento = DateTime.Today.AddYears(-32)
        };

        await _unitOfWork.Pessoas.AddAsync(pessoa);
        await _unitOfWork.SaveChangesAsync();

        // Act - Create both despesa and receita
        var dtoDespesa = new CreateTransacaoDto
        {
            Descricao = "Compra de ações",
            Valor = 1000m,
            Tipo = Transacao.ETipo.Despesa,
            Data = DateTime.Today,
            CategoriaId = categoriaAmbas.Id,
            PessoaId = pessoa.Id
        };

        var resultDespesa = await _transacaoService.CreateAsync(dtoDespesa);

        var dtoReceita = new CreateTransacaoDto
        {
            Descricao = "Dividendos recebidos",
            Valor = 150m,
            Tipo = Transacao.ETipo.Receita,
            Data = DateTime.Today.AddDays(30),
            CategoriaId = categoriaAmbas.Id,
            PessoaId = pessoa.Id
        };

        var resultReceita = await _transacaoService.CreateAsync(dtoReceita);

        // Assert
        resultDespesa.Should().NotBeNull();
        resultDespesa.Tipo.Should().Be(Transacao.ETipo.Despesa);

        resultReceita.Should().NotBeNull();
        resultReceita.Tipo.Should().Be(Transacao.ETipo.Receita);
    }
}
