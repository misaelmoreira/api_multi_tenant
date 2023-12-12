using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ServicoLancamentoNotas.Dominio.SeedWork;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Dominio
{
    
    public class NotiableObjectTests
    {
        [Fact(DisplayName = nameof(Notificar_DeveAdiocionar_NotificacaoNaLisrta))]
        [Trait("Dominio", "NotiableObject - Notificação")]
        public void Notificar_DeveAdiocionar_NotificacaoNaLisrta() 
        {
            //Arrange
            string nomeCampo = "UsuarioId";
            string mensagem = "mensagem teste";
            NotifiableObject objetoNotificavel = new NotaFake();

            //Act
            objetoNotificavel.Notificar(new Notificacao(nomeCampo, mensagem));

            //Arrange
            objetoNotificavel.Notificacoes.Should().NotBeEmpty();
            objetoNotificavel.Notificacoes.Should().HaveCount(1);
            objetoNotificavel.Notificacoes.First().Campo.Should().Be(nomeCampo);
            objetoNotificavel.Notificacoes.First().Mensagem.Should().Be(mensagem);
        }
        
    }
}