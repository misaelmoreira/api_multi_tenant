using System.Threading;
using System.Threading.Tasks;
using ServicoLancamentoNotas.Dominio.Entidades;
using FluentAssertions;
using Moq;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Dominio.Repositories;
using Xunit;
using ServicoLancamentoNotas.Aplicacao.Comum;
using Microsoft.Extensions.Logging;
using System;
using ServicoLancamentoNotas.Aplicacao.Enums;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Lancar
{
    [Collection(nameof(LancarNotaTestsFixture))]
    public class LancarNotaTests
    {
        private readonly LancarNotaTestsFixture _fixture;
        private readonly Mock<INotaRepository> _notaRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<LancarNota>> _logger;
        private readonly ILancarNota _sut;

        public LancarNotaTests(LancarNotaTestsFixture fixture)
        {
            _fixture = fixture;
            _notaRepository = new();
            _unitOfWork = new();
            _logger = new Mock<ILogger<LancarNota>>();
            _sut = new LancarNota(_notaRepository.Object, _unitOfWork.Object, _logger.Object);
        }

        [Fact(DisplayName = nameof(Handle_QuandoNotaNotaValida_DeveSerSalvar))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoNotaNotaValida_DeveSerSalvar()
        {
            var input = _fixture.DevolveNotaInputValido();

            var output = await _sut.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();
            output.Dado.Should().NotBeNull();
            output.Dado.ValorNota.Should().Be(input.ValorNota);
            output.Dado.AtividadeId.Should().Be(input.AtividadeId);
            output.Dado.AlunoId.Should().Be(input.AlunoId);
            _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);            
        }

        [Fact(DisplayName = nameof(Handle_QuandoNotaNotaValidaParaSerSalvaESusbistutiva_DeveSerSalvarEAtualizarNotaSubstituida))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoNotaNotaValidaParaSerSalvaESusbistutiva_DeveSerSalvarEAtualizarNotaSubstituida()
        {
            var nota = _fixture.RetornaNota();
            _notaRepository.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(nota);
            var input = _fixture.DevolveNotaInputValidoSubstitutivo();

            var output = await _sut.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();      
            output.Dado.Should().NotBeNull();
            output.Dado.ValorNota.Should().Be(input.ValorNota);
            output.Dado.AtividadeId.Should().Be(input.AtividadeId);
            output.Dado.AlunoId.Should().Be(input.AlunoId);
            nota.CanceladaPorRetentativa.Should().BeTrue();
            output.Dado.MotivoCancelamento.Should().NotBeEmpty();
            _notaRepository.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));        
            _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _notaRepository.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);    
        }


        [Fact(DisplayName = nameof(Handle_QuandoNotaNotaInvalida_NaoDeveSerSalvar))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoNotaNotaInvalida_NaoDeveSerSalvar()
        {
            var input = _fixture.DevolveNotaInputInvalido();

            var output = await _sut.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();
            output.Dado.Should().BeNull();
            output.Sucesso.Should().BeFalse();
            output.DetalhesErros.Should().NotBeEmpty();
            output.DetalhesErros.Should().HaveCount(3);
            output.Erro.Should().Be(TipoErro.NotaInvalida);
            _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);            
        }

        [Fact(DisplayName = nameof(Handle_QuandoExcecaoInesperada_NaoDeveSerSalvar))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoExcecaoInesperada_NaoDeveSerSalvar()
        {
            var input = _fixture.DevolveNotaInputValido();
            _notaRepository.Setup(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            var output = await _sut.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();
            output.Dado.Should().BeNull();
            output.Sucesso.Should().BeFalse();
            output.Erro.Should().Be(TipoErro.ErroInesperado);
            _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Never);        
        }


        [Fact(DisplayName = nameof(Handle_QuandoNotaNotaValidaParaSerSalvaESusbistutivaNaoEncontrada_DeveSerSalvar))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoNotaNotaValidaParaSerSalvaESusbistutivaNaoEncontrada_DeveSerSalvar()
        {
            _notaRepository.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Nota)null!);
            var input = _fixture.DevolveNotaInputValidoSubstitutivo();

            var output = await _sut.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Should().BeOfType<Resultado<NotaOutputModel>>();      
            output.Dado.Should().NotBeNull();
            output.Dado.ValorNota.Should().Be(input.ValorNota);
            output.Dado.AtividadeId.Should().Be(input.AtividadeId);
            output.Dado.AlunoId.Should().Be(input.AlunoId);
            output.Dado.MotivoCancelamento.Should().NotBeEmpty();
            _notaRepository.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()), Times.Once);        
            _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _notaRepository.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);    
        }
    }
}