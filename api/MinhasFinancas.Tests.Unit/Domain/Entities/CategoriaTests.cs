using Xunit;
using FluentAssertions;
using MinhasFinancas.Domain.Entities;

namespace MinhasFinancas.Tests.Unit.Domain.Entities;

public class CategoriaTests
{
    [Fact]
    public void Constructor_ShouldCreateCategoriaWithValidData()
    {
        // Arrange & Act
        var categoria = new Categoria
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Assert
        categoria.Descricao.Should().Be("Alimentação");
        categoria.Finalidade.Should().Be(Categoria.EFinalidade.Despesa);
        categoria.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void PermiteTipo_ShouldReturnTrueForDespesaInDespesaCategory()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Act
        var permite = categoria.PermiteTipo(Transacao.ETipo.Despesa);

        // Assert
        permite.Should().BeTrue();
    }

    [Fact]
    public void PermiteTipo_ShouldReturnFalseForReceitaInDespesaCategory()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        // Act
        var permite = categoria.PermiteTipo(Transacao.ETipo.Receita);

        // Assert
        permite.Should().BeFalse();
    }

    [Fact]
    public void PermiteTipo_ShouldReturnTrueForReceitaInReceitaCategory()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };

        // Act
        var permite = categoria.PermiteTipo(Transacao.ETipo.Receita);

        // Assert
        permite.Should().BeTrue();
    }

    [Fact]
    public void PermiteTipo_ShouldReturnFalseForDespesaInReceitaCategory()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };

        // Act
        var permite = categoria.PermiteTipo(Transacao.ETipo.Despesa);

        // Assert
        permite.Should().BeFalse();
    }

    [Fact]
    public void PermiteTipo_ShouldReturnTrueForBothTypesInAmbasCategory()
    {
        // Arrange
        var categoria = new Categoria
        {
            Descricao = "Investimentos",
            Finalidade = Categoria.EFinalidade.Ambas
        };

        // Act
        var permiteDespesa = categoria.PermiteTipo(Transacao.ETipo.Despesa);
        var permiteReceita = categoria.PermiteTipo(Transacao.ETipo.Receita);

        // Assert
        permiteDespesa.Should().BeTrue();
        permiteReceita.Should().BeTrue();
    }

    [Fact]
    public void Transacoes_ShouldBeInitializedAsEmptyCollection()
    {
        // Arrange & Act
        var categoria = new Categoria
        {
            Descricao = "Diversos",
            Finalidade = Categoria.EFinalidade.Ambas
        };

        // Assert
        categoria.Transacoes.Should().NotBeNull();
        categoria.Transacoes.Should().BeEmpty();
    }
}
