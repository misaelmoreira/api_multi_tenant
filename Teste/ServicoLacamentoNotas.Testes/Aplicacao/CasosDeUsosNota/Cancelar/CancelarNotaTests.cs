using System.Threading;
using System.Threading.Tasks;
using ServicoLancamentoNotas.Dominio.Entidades;
using FluentAssertions;
using Moq;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Dominio.Repositories;
using Xunit;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Cancelar
{
    [Collection(nameof(CancelarNotaTestsFixture))]
    public class CancelarNotaTests
    {
        private readonly CancelarNotaTestsFixture _fixture;
        private readonly Mock<INotaRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CancelarNota>> _logger;
        private readonly CancelarNota _sut;

        public CancelarNotaTests(CancelarNotaTestsFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new();
            _unitOfWorkMock = new();
            _logger = new();
            _sut = new(_repositoryMock.Object, _unitOfWorkMock.Object, _logger.Object);
        }

        [Fact(DisplayName = nameof(Handle_QuandoCancelarInput_DeveCancelarNota))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoCancelarInput_DeveCancelarNota()
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
            output.Dado.MotivoCancelamento.Should().Be(input.Motivo);
            _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
            _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);            
        }
    }
}