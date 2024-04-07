using FluentAssertions;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Entidades;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Aplicacao.Mapeadores
{
    [Collection(nameof(MapeadorAplicacaoFixture))]
    public class MapeadorAplicacaoTests
    {
        private readonly MapeadorAplicacaoFixture _fixture;

        public MapeadorAplicacaoTests(MapeadorAplicacaoFixture fixture) 
            => _fixture = fixture;

        [Fact(DisplayName = nameof(LancarNotaInputEmNota_QuandoConvertido_DeveRetornarNota))]
        [Trait("Aplicacao", "Mapeadores - LancarNotaInput")]
        public void LancarNotaInputEmNota_QuandoConvertido_DeveRetornarNota()
        {
            //Arrange
            var input = _fixture.DevolveNotaInputValido();

            //Act
            var nota = MapeadorAplicacao.LancarNotaInputEmNota(input);

            //Assert
            nota.Should().BeOfType<Nota>();
            nota.ValorNota.Should().Be(input.ValorNota);
            nota.AlunoId.Should().Be(input.AlunoId);
            nota.AtividadeId.Should().Be(input.AtividadeId);
            nota.StatusIntegracao.Should().Be(StatusIntegracao.AguardandoIntegracao);            
        }  

        [Fact(DisplayName = nameof(NotaEmNotaOutputModel_QuandoConvertido_DeveRetornar))]
        [Trait("Aplicacao", "Mapeadores - NotaOutputModel")]
        public void NotaEmNotaOutputModel_QuandoConvertido_DeveRetornar()
        {
            //Arrange
            var nota = _fixture.RetornaNotaValida();

            //Act
            var outputModel = MapeadorAplicacao.NotaEmNotaOutpuModel(nota);

            //Assert
            outputModel.Should().BeOfType<NotaOutputModel>();
            outputModel.ValorNota.Should().Be(nota.ValorNota);
            outputModel.AlunoId.Should().Be(nota.AlunoId);
            outputModel.AtividadeId.Should().Be(nota.AtividadeId);
            outputModel.StatusIntegracao.Should().Be(nota.StatusIntegracao);            
            outputModel.DataLancamento.Should().Be(nota.DataLancamento);            
            outputModel.Cancelada.Should().Be(nota.Cancelada);            
            outputModel.MotivoCancelamento.Should().Be(nota.MotivoCancelamento);            
        }  
    }
}