using System.Threading;
using System.Threading.Tasks;
using ServicoLancamentoNotas.Dominio.Entidades;
using FluentAssertions;
using Moq;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Dominio.Repositories;
using Xunit;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepositorio;
using System.Collections.Generic;
using System.Linq;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Consultar
{
    [Collection(nameof(ConsultarNotaTestsFixture))]
    public class ConsultarNotaTests
    {
        private readonly ConsultarNotaTestsFixture _fixture;
        private readonly Mock<INotaRepository> _repositoryMock;
        private readonly ConsultarNota _sut;

        public ConsultarNotaTests(ConsultarNotaTestsFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new();
            _sut = new(_repositoryMock.Object);
        }

        [Fact(DisplayName = nameof(Handle_QuandoBuscaRetornaValores_DeveRetornarOutputComValores))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoBuscaRetornaValores_DeveRetornarOutputComValores()
        {
            //arrange
            var buscaInput = _fixture.RetornaListBuscaInput();
            var buscaOutput = _fixture.RetornaOutputRepositorio();
            _repositoryMock.Setup(x => x.Buscar(It.IsAny<BuscaInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(buscaOutput);
            
            //act
            var output = await _sut.Handle(buscaInput, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeAssignableTo<ListaNotaOutput>();
            output.Total.Should().Be(buscaOutput.Total);
            output.Pagina.Should().Be(buscaOutput.Pagina);
            output.PorPagina.Should().Be(buscaOutput.PorPagina);
            output.Items.Should().HaveCount(buscaOutput.Items.Count);
            output.Items.ToList().ForEach(item =>
            {
                var nota = buscaOutput.Items
                        .FirstOrDefault(x => x.AtividadeId == item.AtividadeId && x.AlunoId == item.AlunoId);

                nota.Should().NotBeNull();
                item.ValorNota.Should().Be(nota!.ValorNota);
                item.StatusIntegracao.Should().Be(nota.StatusIntegracao);
                item.Cancelada.Should().Be(nota.Cancelada);
            });     
        }


        [Fact(DisplayName = nameof(Handle_QuandoBuscaNaoRetornaValores_DeveRetornarOutputVazio))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoBuscaNaoRetornaValores_DeveRetornarOutputVazio()
        {
            //arrange
            var buscaInput = _fixture.RetornaListBuscaInput();
            var buscaOutput = _fixture.RetornaOutputRepositorioVazio();
            _repositoryMock.Setup(x => x.Buscar(It.IsAny<BuscaInput>(), It.IsAny<CancellationToken>())).ReturnsAsync(buscaOutput);
            
            //act
            var output = await _sut.Handle(buscaInput, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeAssignableTo<ListaNotaOutput>();
            output.Total.Should().Be(buscaOutput.Total);
            output.Pagina.Should().Be(buscaOutput.Pagina);
            output.PorPagina.Should().Be(buscaOutput.PorPagina);
            output.Items.Should().HaveCount(buscaOutput.Items.Count);     
        }
    }
}