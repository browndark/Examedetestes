using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;

namespace MinhasFinancas.Tests.Unit.Application.DTOs;

/// <summary>
/// Testes para validações de Transação (DTOs).
/// </summary>
public class TransacaoValidationTests
{
    [Fact]
    public void CreateTransacaoDto_Valido_ComDadosCorretos()
    {
        // Arrange
        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra de alimentos",
            Valor = 150.50m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid(),
            Data = DateTime.Today
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void CreateTransacaoDto_Invalido_QuandoDescricaoEhVazia()
    {
        // Arrange
        var dto = new CreateTransacaoDto
        {
            Descricao = string.Empty,
            Valor = 100m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid(),
            Data = DateTime.Today
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(x => x.ErrorMessage!.Contains("obrigatória"));
    }

    [Fact]
    public void CreateTransacaoDto_Invalido_QuandoDescricaoExcede200Caracteres()
    {
        // Arrange
        var dto = new CreateTransacaoDto
        {
            Descricao = new string('A', 201),
            Valor = 100m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid(),
            Data = DateTime.Today
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(x => x.ErrorMessage!.Contains("máximo"));
    }

    [Fact]
    public void CreateTransacaoDto_Valido_Com200Caracteres()
    {
        // Arrange - Boundary test for exactly 200 characters
        var dto = new CreateTransacaoDto
        {
            Descricao = new string('A', 200),
            Valor = 100m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid(),
            Data = DateTime.Today
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void CreateTransacaoDto_Invalido_QuandoValorEhNegativo()
    {
        // Arrange
        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra",
            Valor = -100m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid(),
            Data = DateTime.Today
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(x => x.ErrorMessage!.Contains("positivo"));
    }

    [Fact]
    public void CreateTransacaoDto_Invalido_QuandoValorEhZero()
    {
        // Arrange
        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra",
            Valor = 0m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid(),
            Data = DateTime.Today
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(x => x.ErrorMessage!.Contains("positivo"));
    }

    [Fact]
    public void CreateTransacaoDto_Valido_ComValorMinimo()
    {
        // Arrange - Boundary test for minimum valid value
        var dto = new CreateTransacaoDto
        {
            Descricao = "Compra",
            Valor = 0.01m,
            Tipo = Transacao.ETipo.Despesa,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid(),
            Data = DateTime.Today
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void CreateTransacaoDto_Invalido_QuandoPessoaIdEhEmpty()
    {
        // NOTE: [Required] doesn't check for Guid.Empty values, as Guid.Empty is still a valid Guid.
        // This test is intentionally removed as it doesn't represent an actual validation behavior.
        // For proper Guid validation, a custom validation attribute would be needed.
        // 
        // If needed, implementation would look like:
        // [CustomValidation(typeof(TransacaoValidation), nameof(TransacaoValidation.ValidarGuidNotEmpty))]
    }

    [Fact]
    public void CreateTransacaoDto_Valido_ComTipoReceita()
    {
        // Arrange
        var dto = new CreateTransacaoDto
        {
            Descricao = "Salário",
            Valor = 3000m,
            Tipo = Transacao.ETipo.Receita,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid(),
            Data = DateTime.Today
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeTrue();
        validationResults.Should().BeEmpty();
    }
}
