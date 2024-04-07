using System.Collections.Generic;
using FluentAssertions;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.Enums;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Aplicacao.Comum;

public class ResultadoTests
{
    [Fact(DisplayName = nameof(RetornaSucesso_DeveSetarSucesso))]
    [Trait("Aplicacao", "Resultado - Comum")]
    public void RetornaSucesso_DeveSetarSucesso()
    {
        //arrange
        var dto = new ResultadoDTO("Nome", "Descrição");

        //act
        var resultado = Resultado<ResultadoDTO>.RetornarResultadoSucesso(dto);

        //assert
        resultado.Sucesso.Should().BeTrue();
        resultado.Dado.Should().NotBeNull();
        resultado.Dado.Should().BeAssignableTo<ResultadoDTO>();
        resultado.Erro.Should().BeNull();
        resultado.DescricaoErro.Should().BeNull();
        resultado.DetalhesErros.Should().BeNull();
    }


    [Fact(DisplayName = nameof(RetornaResultadoErro_DeveSetarErro))]
    [Trait("Aplicacao", "Resultado - Comum")]
    public void RetornaResultadoErro_DeveSetarErro()
    {
        //arrange

        //act
        var resultado = Resultado<ResultadoDTO>.RetornaResultadoErro(TipoErro.NotaNaoEncontrada);

        //assert
        resultado.Sucesso.Should().BeFalse();
        resultado.Dado.Should().BeNull();
        resultado.Erro.Should().Be(TipoErro.NotaNaoEncontrada);
        resultado.DescricaoErro.Should().NotBeNull();
        resultado.DetalhesErros.Should().BeNull();
    }


    [Fact(DisplayName = nameof(RetornaResultadoErro_DeveSetarErroDetalhesErro))]
    [Trait("Aplicacao", "Resultado - Comum")]
    public void RetornaResultadoErro_DeveSetarErroDetalhesErro()
    {
        //arrange
        var detalheErros = new List<DetalheErro> { new("Nome", "Campo invalido") };

        //act
        var resultado = Resultado<ResultadoDTO>.RetornaResultadoErro(TipoErro.NotaInvalida, detalheErros);

        //assert
        resultado.Sucesso.Should().BeFalse();
        resultado.Dado.Should().BeNull();
        resultado.Erro.Should().Be(TipoErro.NotaInvalida);
        resultado.DescricaoErro.Should().NotBeNull();
        resultado.DetalhesErros.Should().NotBeEmpty();
        resultado.DetalhesErros.Should().HaveCount(1);
    }

}


public class ResultadoDTO
{
    public ResultadoDTO(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }

    public string Nome { get; set; }
    public string Descricao  { get; set; }
}