using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;

namespace MinhasFinancas.Tests.Unit.Application.DTOs;

/// <summary>
/// Testes para validações de Categoria (DTOs).
/// </summary>
public class CategoriaValidationTests
{
    [Fact]
    public void CreateCategoriaDto_Valido_ComDadosCorretos()
    {
        // Arrange
        var dto = new CreateCategoriaDto
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
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
    public void CreateCategoriaDto_Invalido_QuandoDescricaoEhVazia()
    {
        // Arrange
        var dto = new CreateCategoriaDto
        {
            Descricao = string.Empty,
            Finalidade = Categoria.EFinalidade.Despesa
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
    public void CreateCategoriaDto_Invalido_QuandoDescricaoExcede200Caracteres()
    {
        // Arrange
        var dto = new CreateCategoriaDto
        {
            Descricao = new string('A', 201),
            Finalidade = Categoria.EFinalidade.Despesa
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
    public void CreateCategoriaDto_Valido_Com200Caracteres()
    {
        // Arrange - Boundary test for exactly 200 characters
        var dto = new CreateCategoriaDto
        {
            Descricao = new string('A', 200),
            Finalidade = Categoria.EFinalidade.Despesa
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
    public void CreateCategoriaDto_Valido_ComFinalizadeDespesa()
    {
        // Arrange
        var dto = new CreateCategoriaDto
        {
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
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
    public void CreateCategoriaDto_Valido_ComFinalizadeReceita()
    {
        // Arrange
        var dto = new CreateCategoriaDto
        {
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
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
    public void CreateCategoriaDto_Valido_ComFinalizadeAmbas()
    {
        // Arrange
        var dto = new CreateCategoriaDto
        {
            Descricao = "Transferência",
            Finalidade = Categoria.EFinalidade.Ambas
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
    public void CreateCategoriaDto_Valido_ComNomeUmCaractere()
    {
        // Arrange - Boundary test for minimum description
        var dto = new CreateCategoriaDto
        {
            Descricao = "A",
            Finalidade = Categoria.EFinalidade.Despesa
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
    public void CreateCategoriaDto_Invalido_SemDescricao()
    {
        // Arrange - Null description (different from empty string)
        var dto = new CreateCategoriaDto
        {
            Descricao = null!,
            Finalidade = Categoria.EFinalidade.Despesa
        };

        var validationContext = new ValidationContext(dto);
        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(dto, validationContext, validationResults, validateAllProperties: true);

        // Assert
        isValid.Should().BeFalse();
        validationResults.Should().Contain(x => x.ErrorMessage!.Contains("obrigatória"));
    }
}
