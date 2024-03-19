using System.Threading;
using System.Threading.Tasks;
using ServicoLancamentoNotas.Dominio.Entidades;
using FluentAssertions;
using Moq;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Dominio.Repositories;
using Xunit;
using Microsoft.Extensions.Logging;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.Enums;
using System;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Atualizar
{
    [Collection(nameof(AtualizarNotaTestsFixture))]
    public class AtualizarNotaTests
    {
        private readonly AtualizarNotaTestsFixture _fixture;
        private readonly Mock<INotaRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<AtualizarNota>> _loggerMock;
        private readonly AtualizarNota _sut;

        public AtualizarNotaTests(AtualizarNotaTestsFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new();
            _unitOfWorkMock = new();
            _loggerMock = new();
            _sut = new(_repositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact(DisplayName = nameof(Handle_QuandoAtualizarInput_DeveRetornarResultadoDeSucesso))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoAtualizarInput_DeveRetornarResultadoDeSucesso()
        {
            //arrange
            var nota = _fixture.RetornaNota();
            var input = _fixture.RetornaInputValido();
            _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(nota);

            //act
            var output = await _sut.Handle(input, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();
            output.Sucesso.Should().BeTrue();
            output.Erro.Should().BeNull();
            output.Dado.ValorNota.Should().Be(input.ValorNota);
            _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
            _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);            
        }

        [Fact(DisplayName = nameof(Handle_QuandoNotaNaoEncontrada_DeveRetornarResultadoDeFalha))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoNotaNaoEncontrada_DeveRetornarResultadoDeFalha()
        {
            //arrange
            var input = _fixture.RetornaInputValido();
            _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Nota)null!);

            //act
            var output = await _sut.Handle(input, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();
            output.Sucesso.Should().BeFalse();
            output.Erro.Should().Be(TipoErro.NotaNaoEncontrada);
            output.DescricaoErro.Should().NotBeEmpty();
            output.Dado.Should().BeNull();
            _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);            
        }

        [Fact(DisplayName = nameof(Handle_QuandoLancadaExcecaoNaoEsperada_DeveRetornarResultadoDeFalha))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoLancadaExcecaoNaoEsperada_DeveRetornarResultadoDeFalha()
        {
            //arrange
            var input = _fixture.RetornaInputValido();
            _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            //act
            var output = await _sut.Handle(input, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();
            output.Sucesso.Should().BeFalse();
            output.Erro.Should().Be(TipoErro.ErroInesperado);
            output.DescricaoErro.Should().NotBeEmpty();
            output.Dado.Should().BeNull();
            _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);

        }

        [Fact(DisplayName = nameof(Handle_QuandoNotaInvalida_DeveRetornarResultadoDeFalha))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoNotaInvalida_DeveRetornarResultadoDeFalha()
        {
            //arrange
            var nota = _fixture.RetornaNota();
            var input = _fixture.RetornaInputInvalido();
            _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(nota);

            //act
            var output = await _sut.Handle(input, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();
            output.Sucesso.Should().BeFalse();
            output.Erro.Should().Be(TipoErro.NotaInvalida);
            output.DescricaoErro.Should().NotBeEmpty();
            output.DetalhesErros.Should().NotBeEmpty();
            output.DetalhesErros.Should().HaveCount(1);
            output.Dado.Should().BeNull();
            _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);            
        }
    }
}