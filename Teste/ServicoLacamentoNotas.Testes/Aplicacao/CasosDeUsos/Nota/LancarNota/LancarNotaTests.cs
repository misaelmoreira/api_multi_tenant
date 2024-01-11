using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Dominio.Repositories;
using Xunit;
using CasosDeUsoNota = ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;
using Entidades = Dominio.ServicoLancamentoNotas.Dominio.Entidades;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsos.Nota.LancarNota
{
    [Collection(nameof(LancarNotaTestsFixture))]
    public class LancarNotaTests
    {
        private readonly LancarNotaTestsFixture _fixture;
        private readonly Mock<INotaRepository> _notaRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly ILancarNota _sut;

        public LancarNotaTests(LancarNotaTestsFixture fixture, Mock<INotaRepository> notaRepository, Mock<IUnitOfWork> unitOfWork, ILancarNota sut)
        {
            _fixture = fixture;
            _notaRepository = new Mock<INotaRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _sut = new CasosDeUsoNota.LancarNota(_notaRepository.Object, _unitOfWork.Object);
        }

        [Fact(DisplayName = nameof(Handle_QuandoNotaNotaValidaParaSerSalva_DeveSerSalvar))]
        [Trait("Aplicacao", "Nota - Casos de Uso")]
        public async Task Handle_QuandoNotaNotaValidaParaSerSalva_DeveSerSalvar()
        {
            var input = _fixture.DevolveNotaInputValido();

            var output = await _sut.Handle(input, CancellationToken.None);

            output.Should().NotBeNull();
            output.Should().BeOfType<NotaOutputModel>();
            _notaRepository.Verify(x => x.Inserir(It.IsAny<Entidades.Nota>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);            
        }

    }
}