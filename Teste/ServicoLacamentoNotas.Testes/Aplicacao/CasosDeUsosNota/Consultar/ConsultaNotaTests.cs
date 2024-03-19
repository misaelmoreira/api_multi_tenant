using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ServicoLancamentoNotas.Dominio.Repositories;
using Xunit;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepositorio;
using System.Linq;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using ServicoLancamentoNotas.Aplicacao.Enums;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Consultar
{
    [Collection(nameof(ConsultaNotaTestsFixture))]
    public class ConsultarNotaTests
    {
        private readonly ConsultaNotaTestsFixture _fixture;
        private readonly Mock<INotaRepository> _repositoryMock;
        private readonly Mock<ILogger<ConsultaNota>> _logger;
        private readonly IConsultaNota _sut;

        public ConsultarNotaTests(ConsultaNotaTestsFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new();
            _logger = new();
            _sut = new ConsultaNota(_repositoryMock.Object, _logger.Object);
        }

        [Fact(DisplayName = nameof(Handle_QuandoBuscaRetornaValores_DeveRetornarResultadoComSucessoEValores))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoBuscaRetornaValores_DeveRetornarResultadoComSucessoEValores()
        {
            //arrange
            var buscaInput = _fixture.RetornaListBuscaInput();
            var buscaOutput = _fixture.RetornaOutputRepositorio();
            _repositoryMock.Setup(x => x.Buscar(It.IsAny<BuscaInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(buscaOutput);
            
            //act
            var output = await _sut.Handle(buscaInput, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
            output.Sucesso.Should().BeTrue();
            output.Erro.Should().BeNull();
            output.DescricaoErro.Should().BeNull();
            output.Dado.Total.Should().Be(buscaOutput.Total);
            output.Dado.Pagina.Should().Be(buscaOutput.Pagina);
            output.Dado.PorPagina.Should().Be(buscaOutput.PorPagina);
            output.Dado.Items.Should().HaveCount(buscaOutput.Items.Count);
            output.Dado.Items.ToList().ForEach(item =>
            {
                var nota = buscaOutput.Items
                        .FirstOrDefault(x => x.AtividadeId == item.AtividadeId && x.AlunoId == item.AlunoId);

                nota.Should().NotBeNull();
                item.ValorNota.Should().Be(nota!.ValorNota);
                item.StatusIntegracao.Should().Be(nota.StatusIntegracao);
                item.Cancelada.Should().Be(nota.Cancelada);
            });     
        }


        [Fact(DisplayName = nameof(Handle_QuandoBuscaNaoRetornaValores_DeveRetornarResultadoComSucessoOutputVazio))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoBuscaNaoRetornaValores_DeveRetornarResultadoComSucessoOutputVazio()
        {
            //arrange
            var buscaInput = _fixture.RetornaListBuscaInput();
            var buscaOutput = _fixture.RetornaOutputRepositorioVazio();
            _repositoryMock.Setup(x => x.Buscar(It.IsAny<BuscaInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(buscaOutput);
            
            //act
            var output = await _sut.Handle(buscaInput, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
            output.Sucesso.Should().BeTrue();            
            output.Erro.Should().BeNull();
            output.DescricaoErro.Should().BeNull();
            output.Dado.Total.Should().Be(buscaOutput.Total);
            output.Dado.Pagina.Should().Be(buscaOutput.Pagina);
            output.Dado.PorPagina.Should().Be(buscaOutput.PorPagina);
            output.Dado.Items.Should().HaveCount(buscaOutput.Items.Count);     
        }


        [Fact(DisplayName = nameof(Handle_QuandoBuscaLancaExcecaoNaoEsperada_DeveRetornarResultadoComFalhaEErro))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoBuscaLancaExcecaoNaoEsperada_DeveRetornarResultadoComFalhaEErro()
        {
            //arrange
            var buscaInput = _fixture.RetornaListBuscaInput();
            var buscaOutput = _fixture.RetornaOutputRepositorioVazio();
            _repositoryMock.Setup(x => x.Buscar(It.IsAny<BuscaInput>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            
            //act
            var output = await _sut.Handle(buscaInput, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
            output.Sucesso.Should().BeFalse();            
            output.Erro.Should().NotBeNull();
            output.Erro.Should().Be(TipoErro.ErroInesperado);
            output.DescricaoErro.Should().NotBeNull();  
            output.Dado.Should().BeNull(); 
        }
    }
}