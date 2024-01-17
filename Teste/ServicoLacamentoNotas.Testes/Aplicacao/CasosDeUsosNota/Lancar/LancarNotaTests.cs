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

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Lancar
{
    [Collection(nameof(LancarNotaTestsFixture))]
    public class LancarNotaTests
    {
        private readonly LancarNotaTestsFixture _fixture;
        private readonly Mock<INotaRepository> _notaRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly ILancarNota _sut;

        public LancarNotaTests(LancarNotaTestsFixture fixture)
        {
            _fixture = fixture;
            _notaRepository = new();
            _unitOfWork = new();
            _sut = new LancarNota(_notaRepository.Object, _unitOfWork.Object);
        }

        [Fact(DisplayName = nameof(Handle_QuandoNotaNotaValidaParaSerSalva_DeveSerSalvar))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoNotaNotaValidaParaSerSalva_DeveSerSalvar()
        {
            var input = _fixture.DevolveNotaInputValido();

            var output = await _sut.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Should().BeOfType<NotaOutputModel>();
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
            output.Should().BeOfType<NotaOutputModel>();
            nota.CanceladaPorRetentativa.Should().BeTrue();
            output.MotivoCancelamento.Should().NotBeEmpty();
            _notaRepository.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));        
            _notaRepository.Verify(x => x.Inserir(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _notaRepository.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);    
        }
    }
}