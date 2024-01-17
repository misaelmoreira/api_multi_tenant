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

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Atualizar
{
    [Collection(nameof(AtualizarNotaTestsFixture))]
    public class AtualizarNotaTests
    {
        private readonly AtualizarNotaTestsFixture _fixture;
        private readonly Mock<INotaRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AtualizarNota _sut;

        public AtualizarNotaTests(AtualizarNotaTestsFixture fixture)
        {
            _fixture = fixture;
            _repositoryMock = new();
            _unitOfWorkMock = new();
            _sut = new(_repositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact(DisplayName = nameof(Handle_QuandoAtualizarInput_DeveAtualizarNota))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoAtualizarInput_DeveAtualizarNota()
        {
            //arrange
            var nota = _fixture.RetornaNota();
            var input = _fixture.RetornaInputValido();
            _repositoryMock.Setup(x => x.BuscarNotaPorAlunoEAtividade(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(nota);

            //act
            var output = await _sut.Handle(input, CancellationToken.None);

            //assert
            output.Should().NotBeNull();
            output.Should().BeOfType<NotaOutputModel>();
            output.ValorNota.Should().Be(input.ValorNota);
            _repositoryMock.Verify(x => x.BuscarNotaPorAlunoEAtividade(input.AlunoId, input.AtividadeId, It.IsAny<CancellationToken>()));
            _repositoryMock.Verify(x => x.Atualizar(It.IsAny<Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);            
        }
    }
}