using ServicoLancamentoNotas.Dominio.Entidades;
using FluentAssertions;
using ServicoLacamentoNotas.Testes.Dominio.Entidades;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Specifications;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Dominio.Specifications
{
    [Collection(nameof(NotaTestesFixture))]
    public class StatusSpecificationsTest
    {
        private readonly NotaTestesFixture _fixture;
        public StatusSpecificationsTest(NotaTestesFixture fixture)
            =>  _fixture = fixture;

        [Fact(DisplayName = nameof(Nota_QuandoStatusDeAguardandoIntegracao_DeveSatisfazerEspecificacao))]
        [Trait("Dominio", "StatusNotaSpecification - Specification")]
        public void Nota_QuandoStatusDeAguardandoIntegracao_DeveSatisfazerEspecificacao()
        {
            var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
            Nota nota = new(notaParams);

            StatusAguardandoIntegracaoSpec.Instance.Should().NotBeNull();
            StatusAguardandoIntegracaoSpec.Instance.ToExpression().Should().NotBeNull();
            StatusAguardandoIntegracaoSpec.Instance.IsSatisfied(nota).Should().BeTrue();
        }

        [Theory(DisplayName = nameof(Nota_QuandoStatusDiferenteDeAguardandoIntegracao_NaoDeveSatisfazerEspecificacao))]
        [InlineData(StatusIntegracao.IntegradaComSucesso)]
        [InlineData(StatusIntegracao.FalhaNaIntegracao)]
        [InlineData(StatusIntegracao.EviadaParaIntegracao)]
        [Trait("Dominio", "StatusNotaSpecification - Specification")]
        public void Nota_QuandoStatusDiferenteDeAguardandoIntegracao_NaoDeveSatisfazerEspecificacao(StatusIntegracao statusIntegracao)
        {
            var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(statusIntegracao);
            Nota nota = new(notaParams);

            StatusAguardandoIntegracaoSpec.Instance.Should().NotBeNull();
            StatusAguardandoIntegracaoSpec.Instance.ToExpression().Should().NotBeNull();
            StatusAguardandoIntegracaoSpec.Instance.IsSatisfied(nota).Should().BeFalse();
        }

        [Fact(DisplayName = nameof(Nota_QuandoStatusDeEnviadaParaIntegracao_DeveSatisfazerEspecificacao))]
        [Trait("Dominio", "StatusNotaSpecification - Specification")]
        public void Nota_QuandoStatusDeEnviadaParaIntegracao_DeveSatisfazerEspecificacao()
        {
            var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(StatusIntegracao.EviadaParaIntegracao);
            Nota nota = new(notaParams);

            StatusEnviadaParaIntegracaoSpec.Instance.Should().NotBeNull();
            StatusEnviadaParaIntegracaoSpec.Instance.ToExpression().Should().NotBeNull();
            StatusEnviadaParaIntegracaoSpec.Instance.IsSatisfied(nota).Should().BeTrue();
        }


        [Theory(DisplayName = nameof(Nota_QuandoStatusDiferenteDeEnviadaParaIntegracao_NaoDeveSatisfazerEspecificacao))]
        [InlineData(StatusIntegracao.IntegradaComSucesso)]
        [InlineData(StatusIntegracao.FalhaNaIntegracao)]
        [InlineData(StatusIntegracao.AguardandoIntegracao)]
        [Trait("Dominio", "StatusNotaSpecification - Specification")]
        public void Nota_QuandoStatusDiferenteDeEnviadaParaIntegracao_NaoDeveSatisfazerEspecificacao(StatusIntegracao statusIntegracao)
        {
            var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(statusIntegracao);
            Nota nota = new(notaParams);

            StatusEnviadaParaIntegracaoSpec.Instance.Should().NotBeNull();
            StatusEnviadaParaIntegracaoSpec.Instance.ToExpression().Should().NotBeNull();
            StatusEnviadaParaIntegracaoSpec.Instance.IsSatisfied(nota).Should().BeFalse();
        }        
    }
}