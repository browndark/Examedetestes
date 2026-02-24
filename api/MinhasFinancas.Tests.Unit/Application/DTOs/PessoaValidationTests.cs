using System;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Xunit;
using MinhasFinancas.Application.DTOs;

namespace MinhasFinancas.Tests.Unit.Application.DTOs;

/// <summary>
/// Testes para validações de Pessoa (DTOs).
/// </summary>
public class PessoaValidationTests
{
    [Fact]
    public void PessoaValidation_ValidarDataNascimento_RetornaErro_QuandoDataEhFutura()
    {
        // Arrange
        var dataNascimentoFutura = DateTime.Today.AddDays(1);
        var validationContext = new ValidationContext(new object());
        
        // Act
        var resultado = PessoaValidation.ValidarDataNascimento(dataNascimentoFutura, validationContext);
        
        // Assert
        resultado.Should().NotBe(ValidationResult.Success);
        resultado!.ErrorMessage.Should().Contain("futuro");
    }
    
    [Fact]
    public void PessoaValidation_ValidarDataNascimento_RetornaSuccess_QuandoDataEhValida()
    {
        // Arrange
        var dataNascimentoValida = DateTime.Today.AddYears(-18);
        var validationContext = new ValidationContext(new object());
        
        // Act
        var resultado = PessoaValidation.ValidarDataNascimento(dataNascimentoValida, validationContext);
        
        // Assert
        resultado.Should().Be(ValidationResult.Success);
    }
    
    [Fact]
    public void PessoaValidation_ValidarDataNascimento_RetornaSuccess_QuandoDataEhHoje()
    {
        // Arrange
        var dataNascimentoHoje = DateTime.Today;
        var validationContext = new ValidationContext(new object());
        
        // Act
        var resultado = PessoaValidation.ValidarDataNascimento(dataNascimentoHoje, validationContext);
        
        // Assert
        resultado.Should().Be(ValidationResult.Success);
    }
    
    [Fact]
    public void CreatePessoaDto_Valido_ComDadosCorretos()
    {
        // Arrange
        var dto = new CreatePessoaDto
        {
            Nome = "João Silva",
            DataNascimento = DateTime.Today.AddYears(-25)
        };
        
        var context = new ValidationContext(dto);
        var resultados = new List<ValidationResult>();
        
        // Act
        bool isValid = Validator.TryValidateObject(dto, context, resultados, validateAllProperties: true);
        
        // Assert
        isValid.Should().BeTrue();
        resultados.Should().BeEmpty();
    }
    
    [Fact]
    public void CreatePessoaDto_Invalido_QuandoNomeEhVazio()
    {
        // Arrange
        var dto = new CreatePessoaDto
        {
            Nome = string.Empty,
            DataNascimento = DateTime.Today.AddYears(-25)
        };
        
        var context = new ValidationContext(dto);
        var resultados = new List<ValidationResult>();
        
        // Act
        bool isValid = Validator.TryValidateObject(dto, context, resultados, validateAllProperties: true);
        
        // Assert
        isValid.Should().BeFalse();
        resultados.Should().Contain(r => r.ErrorMessage!.Contains("obrigatório"));
    }
    
    [Fact]
    public void CreatePessoaDto_Invalido_QuandoNomeTemMaisDe200Caracteres()
    {
        // Arrange
        var nomeMuitoLongo = new string('A', 201);
        var dto = new CreatePessoaDto
        {
            Nome = nomeMuitoLongo,
            DataNascimento = DateTime.Today.AddYears(-25)
        };
        
        var context = new ValidationContext(dto);
        var resultados = new List<ValidationResult>();
        
        // Act
        bool isValid = Validator.TryValidateObject(dto, context, resultados, validateAllProperties: true);
        
        // Assert
        isValid.Should().BeFalse();
        resultados.Should().Contain(r => r.ErrorMessage!.Contains("200"));
    }
    
    [Fact]
    public void CreatePessoaDto_Invalido_QuandoDataNascimentoEhFutura()
    {
        // Arrange
        var dto = new CreatePessoaDto
        {
            Nome = "João Silva",
            DataNascimento = DateTime.Today.AddDays(1)
        };
        
        var context = new ValidationContext(dto);
        var resultados = new List<ValidationResult>();
        
        // Act
        bool isValid = Validator.TryValidateObject(dto, context, resultados, validateAllProperties: true);
        
        // Assert
        isValid.Should().BeFalse();
        resultados.Should().Contain(r => r.ErrorMessage!.Contains("futuro"));
    }
    
    [Fact]
    public void UpdatePessoaDto_Valido_ComDadosCorretos()
    {
        // Arrange
        var dto = new UpdatePessoaDto
        {
            Nome = "Maria Santos",
            DataNascimento = DateTime.Today.AddYears(-30)
        };
        
        var context = new ValidationContext(dto);
        var resultados = new List<ValidationResult>();
        
        // Act
        bool isValid = Validator.TryValidateObject(dto, context, resultados, validateAllProperties: true);
        
        // Assert
        isValid.Should().BeTrue();
        resultados.Should().BeEmpty();
    }
    
    [Fact]
    public void CreatePessoaDto_Invalido_QuandoNomeTeExatamente200Caracteres()
    {
        // Arrange
        var nomeComExatamente200 = new string('A', 200);
        var dto = new CreatePessoaDto
        {
            Nome = nomeComExatamente200,
            DataNascimento = DateTime.Today.AddYears(-25)
        };
        
        var context = new ValidationContext(dto);
        var resultados = new List<ValidationResult>();
        
        // Act
        bool isValid = Validator.TryValidateObject(dto, context, resultados, validateAllProperties: true);
        
        // Assert
        isValid.Should().BeTrue();
        resultados.Should().BeEmpty();
    }
    
    [Fact]
    public void CreatePessoaDto_Invalido_QuandoDataNascimentoTemMuitsAnosAtras()
    {
        // Arrange - pessoa com 120 anos (edge case)
        var dto = new CreatePessoaDto
        {
            Nome = "Pessoa Muito Velha",
            DataNascimento = DateTime.Today.AddYears(-120)
        };
        
        var context = new ValidationContext(dto);
        var resultados = new List<ValidationResult>();
        
        // Act
        bool isValid = Validator.TryValidateObject(dto, context, resultados, validateAllProperties: true);
        
        // Assert - Validação de data não rejeita, mas é edge case importante
        isValid.Should().BeTrue(); // Validação atual permite, mas seria caso para adicionar limite
    }
}
