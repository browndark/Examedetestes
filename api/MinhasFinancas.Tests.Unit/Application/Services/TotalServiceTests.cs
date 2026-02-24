using Xunit;
using Moq;
using FluentAssertions;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Domain.ValueObjects;
using MinhasFinancas.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MinhasFinancas.Tests.Unit.Application.Services;

/// <summary>
/// Testes para TotalService.
/// Nota: TotalService depende de ITotaisQuery que geralmente é implementada com raw SQL/queries
/// e é melhor testada em integration tests, porém fazemos testes básicos de comportamento.
/// </summary>
public class TotalServiceTests
{
    private readonly Mock<ITotaisQuery> _mockTotaisQuery;
    private readonly TotalService _totalService;

    public TotalServiceTests()
    {
        _mockTotaisQuery = new Mock<ITotaisQuery>();
        _totalService = new TotalService(_mockTotaisQuery.Object);
    }

    [Fact]
    public async Task GetTotaisPorPessoaAsync_ShouldCallTotaisQueryMethod()
    {
        // Arrange
        var mockResult = new PagedResult<TotalPorPessoa>
        {
            Items = new List<TotalPorPessoa>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 20
        };

        _mockTotaisQuery.Setup(x => x.GetTotaisPorPessoaAsync(It.IsAny<TotaisPorPessoaFilter>(), It.IsAny<PagedRequest>()))
            .ReturnsAsync(mockResult);

        // Act
        var result = await _totalService.GetTotaisPorPessoaAsync();

        // Assert
        result.Should().NotBeNull();
        _mockTotaisQuery.Verify(x => x.GetTotaisPorPessoaAsync(It.IsAny<TotaisPorPessoaFilter>(), It.IsAny<PagedRequest>()), Times.Once);
    }

    [Fact]
    public async Task GetTotaisPorCategoriaAsync_ShouldCallTotaisQueryMethod()
    {
        // Arrange
        var mockResult = new PagedResult<TotalPorCategoria>
        {
            Items = new List<TotalPorCategoria>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 20
        };

        _mockTotaisQuery.Setup(x => x.GetTotaisPorCategoriaAsync(It.IsAny<TotaisPorCategoriaFilter>(), It.IsAny<PagedRequest>()))
            .ReturnsAsync(mockResult);

        // Act
        var result = await _totalService.GetTotaisPorCategoriaAsync();

        // Assert
        result.Should().NotBeNull();
        _mockTotaisQuery.Verify(x => x.GetTotaisPorCategoriaAsync(It.IsAny<TotaisPorCategoriaFilter>(), It.IsAny<PagedRequest>()), Times.Once);
    }

    [Fact]
    public async Task GetTotaisPorPessoaAsync_ShouldPassParametersCorrectly()
    {
        // Arrange
        var filter = new TotaisPorPessoaFilter();
        var pageRequest = new PagedRequest { Page = 2, PageSize = 50 };
        var mockResult = new PagedResult<TotalPorPessoa>
        {
            Items = new List<TotalPorPessoa>(),
            TotalCount = 0,
            Page = 2,
            PageSize = 50
        };

        _mockTotaisQuery.Setup(x => x.GetTotaisPorPessoaAsync(filter, pageRequest))
            .ReturnsAsync(mockResult);

        // Act
        var result = await _totalService.GetTotaisPorPessoaAsync(filter, pageRequest);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(50);
        _mockTotaisQuery.Verify(x => x.GetTotaisPorPessoaAsync(filter, pageRequest), Times.Once);
    }

    [Fact]
    public async Task GetTotaisPorCategoriaAsync_ShouldPassParametersCorrectly()
    {
        // Arrange
        var filter = new TotaisPorCategoriaFilter();
        var pageRequest = new PagedRequest { Page = 1, PageSize = 25 };
        var mockResult = new PagedResult<TotalPorCategoria>
        {
            Items = new List<TotalPorCategoria>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 25
        };

        _mockTotaisQuery.Setup(x => x.GetTotaisPorCategoriaAsync(filter, pageRequest))
            .ReturnsAsync(mockResult);

        // Act
        var result = await _totalService.GetTotaisPorCategoriaAsync(filter, pageRequest);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(25);
        _mockTotaisQuery.Verify(x => x.GetTotaisPorCategoriaAsync(filter, pageRequest), Times.Once);
    }
}

