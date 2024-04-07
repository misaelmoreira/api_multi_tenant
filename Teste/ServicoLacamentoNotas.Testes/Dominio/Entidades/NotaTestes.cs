using System;
using System.Linq;
using ServicoLancamentoNotas.Dominio.Entidades;
using FluentAssertions;
using ServicoLancamentoNotas.Dominio.Constantes;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.SeedWork;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Dominio.Entidades
{
    [Collection(nameof(NotaTestesFixture))]
    public class NotaTestes
    {
        private readonly NotaTestesFixture _fixture;
        public NotaTestes(NotaTestesFixture fixture)
        {
            _fixture = fixture;            
        }

        [Fact(DisplayName = "")]
        [Trait("Dominio", "Nota - Agregado")]
        public void InstanciarNota()
        {
            //Arrange
            var parametrosNota = _fixture.RetornaValoresParametrosNotaValidos(); 

            //Act
            var nota = new Nota(parametrosNota);

            //Assert
            nota.Should().NotBeNull();
            nota.Id.Should().NotBeEmpty();
            nota.Id.Should().NotBe(Guid.Empty);
            nota.AlunoId.Should().Be(parametrosNota.AlunoId);
            nota.AtividadeId.Should().Be(parametrosNota.AtividadeId);
            nota.ValorNota.Should().Be(parametrosNota.ValorNota);            
            nota.DataLancamento.Should().Be(parametrosNota.DataLancamento);
            nota.DataLancamento.Should().NotBe(default);
            nota.DataCriacao.Should().NotBe(default);
            nota.DataCriacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            nota.UsuarioId.Should().Be(parametrosNota.UsuarioId);
            nota.CanceladaPorRetentativa.Should().BeFalse();
            nota.Cancelada.Should().BeFalse();
            nota.StatusIntegracao.Should().Be(parametrosNota.StatusIntegracao);
            nota.MotivoCancelamento.Should().BeNull();
            nota.Should().BeAssignableTo<NotifiableObject>();
            nota.EhValida.Should().BeTrue();
        }
        
        [Theory(DisplayName = nameof(InstanciarNota_QuandoValorNotaInvalido_DeveLancarNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]
        [InlineData(-1)]
        [InlineData(11)]
        public void InstanciarNota_QuandoValorNotaInvalido_DeveLancarNotificacao(double valorNota)
        {
            //Arrange
            var parametroNota = _fixture.RetornaValoresParametrosInvalidosCustomizados(valorNota : valorNota);

            //Act      
            var nota = new Nota(parametroNota);
            
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();
            nota.Notificacoes.Should().HaveCount(1);
            nota.Notificacoes.First().Campo.Should().Be(nameof(nota.ValorNota));
            nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
            nota.EhValida.Should().BeFalse();
        }        

        [Theory(DisplayName = nameof(InstanciarNota_QuandoAlunoIdInvalido_DeveLancarNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]
        [InlineData(-1)]
        [InlineData(0)]
        public void InstanciarNota_QuandoAlunoIdInvalido_DeveLancarNotificacao(int alunoId)
        {
            //Arrange
            var parametroAluno = _fixture.RetornaValoresParametrosInvalidosCustomizados(alunoId : alunoId);

            //Act      
            var nota = new Nota(parametroAluno);
            
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();
            nota.Notificacoes.Should().HaveCount(1);
            nota.Notificacoes.First().Campo.Should().Be(nameof(nota.AlunoId));
            nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO);
            nota.EhValida.Should().BeFalse();
        }

        [Theory(DisplayName = nameof(InstanciarNota_QuandoAtividadeIdInvalido_DeveLancarNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]
        [InlineData(-1)]
        [InlineData(0)]
        public void InstanciarNota_QuandoAtividadeIdInvalido_DeveLancarNotificacao(int atividadeId)
        {
            //Arrange
            var parametroAtividade = _fixture.RetornaValoresParametrosInvalidosCustomizados(atividadeId : atividadeId);

            //Act      
            var nota = new Nota(parametroAtividade);
            
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();
            nota.Notificacoes.Should().HaveCount(1);
            nota.Notificacoes.First().Campo.Should().Be(nameof(nota.AtividadeId));
            nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDO);
            nota.EhValida.Should().BeFalse();
        }

        [Theory(DisplayName = nameof(CancelaNotaQuandoNaoInformadoMotivo_DevePossuirNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]            
        [InlineData("")]
        [InlineData(null)]
        [InlineData("  ")]
        public void CancelaNotaQuandoNaoInformadoMotivo_DevePossuirNotificacao(string motivoCancelamento)
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
            Nota nota = new(notaParams);

            //Act      
            nota.Cancelar(motivoCancelamento);
            
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();
            nota.Notificacoes.Should().HaveCount(1);
            nota.Notificacoes.First().Campo.Should().Be(nameof(nota.MotivoCancelamento));
            nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_MOTIVO_CANCELAMENTO_NAO_INFORMADO);
            nota.EhValida.Should().BeFalse();
            nota.Cancelada.Should().BeFalse();
        }

        [Fact(DisplayName = nameof(CancelaNota_QuandoInformadoMotivoExtenso_DevePossuirNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]        
        public void CancelaNota_QuandoInformadoMotivoExtenso_DevePossuirNotificacao()
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
            Nota nota = new(notaParams);
            string motivoCancelamento = _fixture.Faker.Lorem.Text();
            while(motivoCancelamento.Length <= 500)
                motivoCancelamento += _fixture.Faker.Lorem.Text();

            //Act      
            nota.Cancelar(motivoCancelamento);
            
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();
            nota.Notificacoes.Should().HaveCount(1);
            nota.Notificacoes.First().Campo.Should().Be(nameof(nota.MotivoCancelamento));
            nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_MOTIVO_CANCELAMENTO_EXTENSO);
            nota.EhValida.Should().BeFalse();
        }

        [Fact(DisplayName = nameof(CancelaNota_QuandoInformadoMotivo_NaoDevePossuirNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]        
        public void CancelaNota_QuandoInformadoMotivo_NaoDevePossuirNotificacao()
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
            Nota nota = new(notaParams);
            string motivoCancelamento = _fixture.Faker.Lorem.Text();
            
            //Act      
            nota.Cancelar(motivoCancelamento);
            
            //Assert
            nota.Notificacoes.Should().BeEmpty();            
            nota.EhValida.Should().BeTrue();
            nota.Cancelada.Should().BeTrue();
            nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName = nameof(CancelarPorRetentativa_QuandoSolicitadoCancelamento_NaoDevePossuirNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]        
        public void CancelarPorRetentativa_QuandoSolicitadoCancelamento_NaoDevePossuirNotificacao()
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
            Nota nota = new(notaParams);
                        
            //Act      
            nota.CancelarPorRetentativa();
            
            //Assert
            nota.Notificacoes.Should().BeEmpty();            
            nota.EhValida.Should().BeTrue();
            nota.Cancelada.Should().BeTrue();
            nota.CanceladaPorRetentativa.Should().BeTrue();
            nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Theory(DisplayName = nameof(AtualizarValorNota_QuandoInformadoValoresInvalido_DevePossuirNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]        
        [InlineData(-1)]
        [InlineData(11)]
        public void AtualizarValorNota_QuandoInformadoValoresInvalido_DevePossuirNotificacao(double novoValorNota)
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
            Nota nota = new(notaParams);
                        
            //Act      
            nota.AtualizarValorNota(novoValorNota);
            
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();            
            nota.EhValida.Should().BeFalse();
            nota.Notificacoes.Should().HaveCount(1);
            nota.Notificacoes.First().Campo.Should().Be(nameof(nota.ValorNota));
            nota.Notificacoes.First().Mensagem.Should().Be(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
        }

        [Theory(DisplayName = nameof(AtualizarValorNota_QuandoInformadoValoresValido_NaoDevePossuirNotificacao))]
        [Trait("Dominio", "Nota - Agregado")]        
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(10)]
        public void AtualizarValorNota_QuandoInformadoValoresValido_NaoDevePossuirNotificacao(double novoValorNota)
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
            Nota nota = new(notaParams);
                        
            //Act      
            nota.AtualizarValorNota(novoValorNota);
            
            //Assert
            nota.Notificacoes.Should().BeEmpty();            
            nota.EhValida.Should().BeTrue();
            nota.ValorNota.Should().Be(novoValorNota);
            nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact(DisplayName = nameof(AlterarStatusParaEnviada_QuandoPermitidoTrocaStatus_DeveAtualizarOStatus))]
        [Trait("Dominio", "Nota - Agregado")]        
        public void AlterarStatusParaEnviada_QuandoPermitidoTrocaStatus_DeveAtualizarOStatus()
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidos();
            Nota nota = new(notaParams);
            
            //Act      
            nota.AlterarStatusIntegracaoParaEnviada();
            
            //Assert
            nota.Notificacoes.Should().BeEmpty();            
            nota.EhValida.Should().BeTrue();            
            nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            nota.StatusIntegracao.Should().Be(StatusIntegracao.EviadaParaIntegracao);
        }

        [Theory(DisplayName = nameof(AlterarStatusParaEnviada_QuandoNaoPermitidoTrocaStatus_DeveAdcionarNotificacao))]
        [InlineData(StatusIntegracao.EviadaParaIntegracao)]
        [InlineData(StatusIntegracao.IntegradaComSucesso)]
        [InlineData(StatusIntegracao.FalhaNaIntegracao)]
        [Trait("Dominio", "Nota - Agregado")]        
        public void AlterarStatusParaEnviada_QuandoNaoPermitidoTrocaStatus_DeveAdcionarNotificacao(StatusIntegracao statusIntegracao)
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(statusIntegracao);
            Nota nota = new(notaParams);
            
            //Act      
            nota.AlterarStatusIntegracaoParaEnviada();
            
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();            
            nota.EhValida.Should().BeFalse();
            nota.Notificacoes.Should().HaveCount(1);
        }

        [Fact(DisplayName = nameof(AlterarStatusParaFalhaIntegracao_QuandoPermitidoTrocaStatus_DeveAtualizarOStatus))]
        [Trait("Dominio", "Nota - Agregado")]        
        public void AlterarStatusParaFalhaIntegracao_QuandoPermitidoTrocaStatus_DeveAtualizarOStatus()
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(StatusIntegracao.EviadaParaIntegracao);
            Nota nota = new(notaParams);
            
            //Act      
            nota.AlterarStatusIntegracaoParaFalhaIntegracao();
                
            //Assert
            nota.Notificacoes.Should().BeEmpty();            
            nota.EhValida.Should().BeTrue();            
            nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            nota.StatusIntegracao.Should().Be(StatusIntegracao.FalhaNaIntegracao);
        }

        [Theory(DisplayName = nameof(AlterarStatusParaFalhaIntegracao_QuandoNaoPermitidoTrocaStatus_NaoDeveAtualizarOStatus))]
        [InlineData(StatusIntegracao.AguardandoIntegracao)]
        [InlineData(StatusIntegracao.IntegradaComSucesso)]
        [InlineData(StatusIntegracao.FalhaNaIntegracao)]
        [Trait("Dominio", "Nota - Agregado")]        
        public void AlterarStatusParaFalhaIntegracao_QuandoNaoPermitidoTrocaStatus_NaoDeveAtualizarOStatus(StatusIntegracao statusIntegracao)
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(statusIntegracao);
            Nota nota = new(notaParams);
            
            //Act      
            nota.AlterarStatusIntegracaoParaFalhaIntegracao();
                        
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();            
            nota.EhValida.Should().BeFalse();
            nota.Notificacoes.Should().HaveCount(1);
        }

        [Fact(DisplayName = nameof(AlterarStatusParaIntegradaComSucesso_QuandoPermitidoTrocaStatus_DeveAtualizarOStatus))]
        [Trait("Dominio", "Nota - Agregado")]        
        public void AlterarStatusParaIntegradaComSucesso_QuandoPermitidoTrocaStatus_DeveAtualizarOStatus()
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(StatusIntegracao.EviadaParaIntegracao);
            Nota nota = new(notaParams);
            
            //Act      
            nota.AlterarStatusParaIntegradaComSucesso();
                
            //Assert
            nota.Notificacoes.Should().BeEmpty();            
            nota.EhValida.Should().BeTrue();            
            nota.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            nota.StatusIntegracao.Should().Be(StatusIntegracao.IntegradaComSucesso);
        }

        [Theory(DisplayName = nameof(AlterarStatusParaIntegradaComSucesso_QuandoNaoPermitidoTrocaStatus_NaoDeveAtualizarOStatus))]
        [InlineData(StatusIntegracao.AguardandoIntegracao)]
        [InlineData(StatusIntegracao.IntegradaComSucesso)]
        [InlineData(StatusIntegracao.FalhaNaIntegracao)]
        [Trait("Dominio", "Nota - Agregado")]        
        public void AlterarStatusParaIntegradaComSucesso_QuandoNaoPermitidoTrocaStatus_NaoDeveAtualizarOStatus(StatusIntegracao statusIntegracao)
        {
            //Arrange
            var notaParams = _fixture.RetornaValoresParametrosNotaValidosComStatus(statusIntegracao);
            Nota nota = new(notaParams);
            
            //Act      
            nota.AlterarStatusParaIntegradaComSucesso();
                        
            //Assert
            nota.Notificacoes.Should().NotBeEmpty();            
            nota.EhValida.Should().BeFalse();
            nota.Notificacoes.Should().HaveCount(1);    
        }
       
    }
}