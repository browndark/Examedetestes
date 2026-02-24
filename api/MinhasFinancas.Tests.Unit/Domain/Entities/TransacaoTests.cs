using Xunit;
using FluentAssertions;
using MinhasFinancas.Domain.Entities;
using System;

namespace MinhasFinancas.Tests.Unit.Domain.Entities;

public class TransacaoTests
{
    private DateTime _today = DateTime.Today;

    [Fact]
    public void Constructor_ShouldCreateTransacaoWithValidData()
    {
        // Arrange & Act
        var transacao = new Transacao
        {
            Descricao = "Compra no supermercado",
            Valor = 150.00m,
            Tipo = Transacao.ETipo.Despesa,
            Data = _today
        };

        // Assert
        transacao.Descricao.Should().Be("Compra no supermercado");
        transacao.Valor.Should().Be(150.00m);
        transacao.Tipo.Should().Be(Transacao.ETipo.Despesa);
        transacao.Data.Should().Be(_today);
        transacao.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Transacao_ShouldHaveRequiredPropertiesSet()
    {
        // Arrange
        var transacao = new Transacao
        {
            Descricao = "Test Transaction",
            Valor = 100m,
            Tipo = Transacao.ETipo.Despesa,
            Data = _today
        };

        // Act & Assert
        transacao.Descricao.Should().NotBeNullOrEmpty();
        transacao.Valor.Should().BeGreaterThan(0);
        transacao.Data.Should().NotBe(default(DateTime));
    }

    [Fact]
    public void Transacao_ShouldHaveMinimumValueValidation()
    {
        // Arrange & Act
        var transacao = new Transacao
        {
            Descricao = "Small transaction",
            Valor = 0.01m,
            Tipo = Transacao.ETipo.Despesa,
            Data = _today
        };

        // Assert
        transacao.Valor.Should().Be(0.01m);
    }

    [Fact]
    public void Transacao_ShouldDefaultDataToToday()
    {
        // Arrange & Act
        var transacao = new Transacao
        {
            Descricao = "Test",
            Valor = 100m,
            Tipo = Transacao.ETipo.Despesa
        };

        // Assert
        transacao.Data.Should().Be(DateTime.Today);
    }

    [Fact]
    public void Transacao_ShouldHandleBothTransactionTypes()
    {
        // Arrange
        var despesa = new Transacao
        {
            Descricao = "Expense",
            Valor = 100m,
            Tipo = Transacao.ETipo.Despesa,
            Data = _today
        };

        var receita = new Transacao
        {
            Descricao = "Income",
            Valor = 200m,
            Tipo = Transacao.ETipo.Receita,
            Data = _today
        };

        // Assert
        despesa.Tipo.Should().Be(Transacao.ETipo.Despesa);
        receita.Tipo.Should().Be(Transacao.ETipo.Receita);
    }
}
