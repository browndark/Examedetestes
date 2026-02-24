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

public class PessoaServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly PessoaService _pessoaService;

    public PessoaServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _pessoaService = new PessoaService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreatePessoaWithValidData()
    {
        // Arrange
        var dto = new CreatePessoaDto
        {
            Nome = "João Silva",
            DataNascimento = new DateTime(1990, 5, 15)
        };

        var pessoaId = Guid.NewGuid();

        _mockUnitOfWork.Setup(x => x.Pessoas.AddAsync(It.IsAny<Pessoa>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(0);

        // Act
        var result = await _pessoaService.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Nome.Should().Be("João Silva");
        result.DataNascimento.Should().Be(new DateTime(1990, 5, 15));
        _mockUnitOfWork.Verify(x => x.Pessoas.AddAsync(It.IsAny<Pessoa>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPessoaWhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pessoa = new Pessoa
        {
            Id = id,
            Nome = "Ana Clara",
            DataNascimento = new DateTime(1995, 3, 20)
        };

        _mockUnitOfWork.Setup(x => x.Pessoas.GetByIdAsync(id))
            .ReturnsAsync(pessoa);

        // Act
        var result = await _pessoaService.GetByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Nome.Should().Be("Ana Clara");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullWhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockUnitOfWork.Setup(x => x.Pessoas.GetByIdAsync(id))
            .ReturnsAsync((Pessoa)null!);

        // Act
        var result = await _pessoaService.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePessoa()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockUnitOfWork.Setup(x => x.Pessoas.DeleteAsync(id))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(0);

        // Act
        await _pessoaService.DeleteAsync(id);

        // Assert
        _mockUnitOfWork.Verify(x => x.Pessoas.DeleteAsync(id), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionWhenDtoIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _pessoaService.CreateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowExceptionWhenPessoaNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var updateDto = new UpdatePessoaDto
        {
            Nome = "Novo Nome",
            DataNascimento = DateTime.Today.AddYears(-25)
        };

        _mockUnitOfWork.Setup(x => x.Pessoas.GetByIdAsync(id))
            .ReturnsAsync((Pessoa)null!);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _pessoaService.UpdateAsync(id, updateDto));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedResultOfPessoas()
    {
        // Arrange
        var pessoas = new List<Pessoa>
        {
            new Pessoa { Id = Guid.NewGuid(), Nome = "João", DataNascimento = new DateTime(1990, 5, 15) },
            new Pessoa { Id = Guid.NewGuid(), Nome = "Maria", DataNascimento = new DateTime(1995, 3, 20) }
        };

        var expectedResult = new MinhasFinancas.Domain.ValueObjects.PagedResult<PessoaDto>
        {
            Items = pessoas.Select(p => new PessoaDto
            {
                Id = p.Id,
                Nome = p.Nome,
                DataNascimento = p.DataNascimento,
                Idade = p.Idade
            }).ToList(),
            TotalCount = 2,
            Page = 1,
            PageSize = 20
        };

        _mockUnitOfWork.Setup(x => x.Pessoas.GetPagedAsync(It.IsAny<MinhasFinancas.Domain.ValueObjects.PagedRequest>(), It.IsAny<MinhasFinancas.Domain.Interfaces.ISpecification<Pessoa, PessoaDto>>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _pessoaService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Page.Should().Be(1);
    }
}
