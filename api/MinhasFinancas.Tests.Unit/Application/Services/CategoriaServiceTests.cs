using Xunit;
using Moq;
using FluentAssertions;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace MinhasFinancas.Tests.Unit.Application.Services;

/// <summary>
/// Testes para CategoriaService.
/// </summary>
public class CategoriaServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CategoriaService _categoriaService;

    public CategoriaServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _categoriaService = new CategoriaService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCategoriaWithValidData()
    {
        // Arrange
        var dto = new CreateCategoriaDto
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        _mockUnitOfWork.Setup(x => x.Categorias.AddAsync(It.IsAny<Categoria>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(0);

        // Act
        var result = await _categoriaService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Descricao.Should().Be("Alimentação");
        result.Finalidade.Should().Be(Categoria.EFinalidade.Despesa);
        _mockUnitOfWork.Verify(x => x.Categorias.AddAsync(It.IsAny<Categoria>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategoriaWhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categoria = new Categoria
        {
            Id = id,
            Descricao = "Transporte",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        _mockUnitOfWork.Setup(x => x.Categorias.GetByIdAsync(id))
            .ReturnsAsync(categoria);

        // Act
        var result = await _categoriaService.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Descricao.Should().Be("Transporte");
        result.Finalidade.Should().Be(Categoria.EFinalidade.Despesa);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullWhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockUnitOfWork.Setup(x => x.Categorias.GetByIdAsync(id))
            .ReturnsAsync((Categoria)null!);

        // Act
        var result = await _categoriaService.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionWhenDtoIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _categoriaService.CreateAsync(null!));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCategoriaReceitaType()
    {
        // Arrange
        var dto = new CreateCategoriaDto
        {
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };

        _mockUnitOfWork.Setup(x => x.Categorias.AddAsync(It.IsAny<Categoria>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(0);

        // Act
        var result = await _categoriaService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Finalidade.Should().Be(Categoria.EFinalidade.Receita);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCategoriaBothType()
    {
        // Arrange - Tests the "Ambas" finalidade
        var dto = new CreateCategoriaDto
        {
            Descricao = "Transferência",
            Finalidade = Categoria.EFinalidade.Ambas
        };

        _mockUnitOfWork.Setup(x => x.Categorias.AddAsync(It.IsAny<Categoria>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(0);

        // Act
        var result = await _categoriaService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Finalidade.Should().Be(Categoria.EFinalidade.Ambas);
    }
}
