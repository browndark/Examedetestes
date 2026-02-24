using Xunit;
using FluentAssertions;
using MinhasFinancas.Domain.Entities;
using System;

namespace MinhasFinancas.Tests.Unit.Domain.Entities;

public class PessoaTests
{
    [Fact]
    public void Constructor_ShouldCreatePessoaWithValidData()
    {
        // Arrange & Act
        var pessoa = new Pessoa
        {
            Nome = "João Silva",
            DataNascimento = new DateTime(1990, 5, 15)
        };

        // Assert
        pessoa.Nome.Should().Be("João Silva");
        pessoa.DataNascimento.Should().Be(new DateTime(1990, 5, 15));
        pessoa.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Idade_ShouldCalculateCorrectlyForAdult()
    {
        // Arrange
        var birthDate = DateTime.Today.AddYears(-25);
        var pessoa = new Pessoa
        {
            Nome = "Ana Costa",
            DataNascimento = birthDate
        };

        // Act
        var idade = pessoa.Idade;

        // Assert
        idade.Should().Be(25);
    }

    [Fact]
    public void Idade_ShouldCalculateCorrectlyForMinor()
    {
        // Arrange
        var birthDate = DateTime.Today.AddYears(-15);
        var pessoa = new Pessoa
        {
            Nome = "Pedro Junior",
            DataNascimento = birthDate
        };

        // Act
        var idade = pessoa.Idade;

        // Assert
        idade.Should().Be(15);
    }

    [Fact]
    public void EhMaiorDeIdade_ShouldReturnTrueForAdult()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Maria Lopez",
            DataNascimento = DateTime.Today.AddYears(-21)
        };

        // Act
        var isMajor = pessoa.EhMaiorDeIdade();

        // Assert
        isMajor.Should().BeTrue();
    }

    [Fact]
    public void EhMaiorDeIdade_ShouldReturnFalseForMinor()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Carlos Menino",
            DataNascimento = DateTime.Today.AddYears(-10).AddMonths(-6)
        };

        // Act
        var isMajor = pessoa.EhMaiorDeIdade();

        // Assert
        isMajor.Should().BeFalse();
    }

    [Fact]
    public void EhMaiorDeIdade_ShouldReturnTrueForExactly18Years()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Gabriel",
            DataNascimento = DateTime.Today.AddYears(-18)
        };

        // Act
        var isMajor = pessoa.EhMaiorDeIdade();

        // Assert
        isMajor.Should().BeTrue();
    }

    [Fact]
    public void EhMaiorDeIdade_ShouldReturnFalseForJust17Years()
    {
        // Arrange
        var pessoa = new Pessoa
        {
            Nome = "Julia",
            DataNascimento = DateTime.Today.AddYears(-17).AddDays(1)
        };

        // Act
        var isMajor = pessoa.EhMaiorDeIdade();

        // Assert
        isMajor.Should().BeFalse();
    }

    [Fact]
    public void Transacoes_ShouldBeInitalizedAsEmptyCollection()
    {
        // Arrange & Act
        var pessoa = new Pessoa
        {
            Nome = "Bruno Santos",
            DataNascimento = DateTime.Today.AddYears(-30)
        };

        // Assert
        pessoa.Transacoes.Should().NotBeNull();
        pessoa.Transacoes.Should().BeEmpty();
    }
}
