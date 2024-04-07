using System.Linq;
using FluentAssertions;
using ServicoLacamentoNotas.Testes.Dominio.Entidades;
using ServicoLacamentoNotas.Testes.Validacoes.Validador;
using ServicoLancamentoNotas.Dominio.Constantes;
using ServicoLancamentoNotas.Dominio.SeedWork;
using ServicoLancamentoNotas.Dominio.Validacoes;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Validacoes
{
    [Collection(nameof(NotaTestesFixture))]
    public class ValidacaoDominioTeste
    {
        private readonly NotaTestesFixture _fixture;
        public ValidacaoDominioTeste(NotaTestesFixture fixture)
        {
            _fixture = fixture;            
        }

        [Theory(DisplayName = nameof(DeveEstarEntre_QuandoValorEstaNoIntervalo_NaoDeveNotificarObjeto))]
        [Trait("Dominio", "ValidacoesDominio - Validacoes")]
        [InlineData(0)]
        [InlineData(5)]
        [InlineData(10)]
        public void DeveEstarEntre_QuandoValorEstaNoIntervalo_NaoDeveNotificarObjeto(double valor)
        {
            //Arrange
            var valorInicialIntervalo = default(double);
            var valorFinalIntervalo = 10;
            NotifiableObject objetoNotificavel = new NotaFake();

            //Act
            ValidacoesDominio.DeveEstarEntre(valor, valorInicialIntervalo, valorFinalIntervalo, objetoNotificavel, null!, null!);

            //Assert
            objetoNotificavel.Notificacoes.Should().BeEmpty();
            objetoNotificavel.Notificacoes.Should().HaveCount(default(int));
        }

        [Theory(DisplayName = nameof(DeveEstarEntre_QuandoValorEstaForaDoIntervalo_DeveNotificarObjeto))]
        [Trait("Dominio", "ValidacoesDominio - Validacoes")]
        [InlineData(-1)]
        [InlineData(11)]
        public void DeveEstarEntre_QuandoValorEstaForaDoIntervalo_DeveNotificarObjeto(double valor)
        {
            //Arrange
            var valorInicialIntervalo = default(double);
            var valorFinalIntervalo = 10;
            var mensagem = "O valor está fora do intervalo";
            var nomeCampo = "ValorNota";
            NotifiableObject objetoNotificavel = new NotaFake();

            //Act
            ValidacoesDominio.DeveEstarEntre(valor, valorInicialIntervalo, valorFinalIntervalo, objetoNotificavel, nomeCampo, mensagem);

            //Assert
            objetoNotificavel.Notificacoes.Should().NotBeEmpty();
            objetoNotificavel.Notificacoes.Should().HaveCount(1);
            objetoNotificavel.Notificacoes.First().Campo.Should().Be(nomeCampo);
            objetoNotificavel.Notificacoes.First().Mensagem.Should().Be(mensagem);
        }

        [Theory(DisplayName = nameof(MaiorQue_QuandoValorMenorOuIgual_DeveNotificarObjeto))]
        [Trait("Dominio", "ValidacoesDominio - Validacoes")]
        [InlineData(-1)]
        [InlineData(0)]
        public void MaiorQue_QuandoValorMenorOuIgual_DeveNotificarObjeto(int valor)
        {
            //Arrange
            var valorMinimo = default(int);
            var mensagem = "O usuario está invalido";
            var nomeCampo = "UsuarioId";
            NotifiableObject objetoNotificavel = new NotaFake();

            //Act
            ValidacoesDominio.MaiorQue(valor, valorMinimo, objetoNotificavel, nomeCampo, mensagem);

            //Assert
            objetoNotificavel.Notificacoes.Should().NotBeEmpty();
            objetoNotificavel.Notificacoes.Should().HaveCount(1);
            objetoNotificavel.Notificacoes.First().Campo.Should().Be(nomeCampo);
            objetoNotificavel.Notificacoes.First().Mensagem.Should().Be(mensagem);
        }

        [Theory(DisplayName = nameof(MaiorQue_QuandoValorMaior_NaoDeveNotificarObjeto))]
        [Trait("Dominio", "ValidacoesDominio - Validacoes")]
        [InlineData(10)]
        [InlineData(1550)]
        public void MaiorQue_QuandoValorMaior_NaoDeveNotificarObjeto(int valor)
        {
            //Arrange
            var valorMinimo = default(int);
            var mensagem = "O usuario está invalido";
            var nomeCampo = "UsuarioId";
            NotifiableObject objetoNotificavel = new NotaFake();

            //Act
            ValidacoesDominio.MaiorQue(valor, valorMinimo, objetoNotificavel, nomeCampo, mensagem);

            //Assert
            objetoNotificavel.Notificacoes.Should().BeEmpty();
            objetoNotificavel.Notificacoes.Should().HaveCount(default(int));
        }
        
        [Fact(DisplayName = "")]
        [Trait("Dominio", "ValidacoesDominio - Validacoes")]
        public void NumeroMaximoCaracteres_QuandoValorExcedeQuantidadeCaracteres_DeveNotificarObjeto()
        {
            //Arrange
            var texto = _fixture.Faker.Lorem.Text();
            while(texto.Length <= 500)
                texto += _fixture.Faker.Lorem.Text();

            var numeroMaximoCaracteres = 500;
            var mensagem = "A quantidade de caracteres foi excedida";
            var nomeCampo = "MotivoCancelamento";
            NotifiableObject objetoNotificavel = new NotaFake();

            //Act
            ValidacoesDominio.NumeroMaximoCaracteres(texto, numeroMaximoCaracteres, objetoNotificavel, nomeCampo, mensagem);

            //Assert
            objetoNotificavel.Notificacoes.Should().NotBeEmpty();
            objetoNotificavel.Notificacoes.Should().HaveCount(1);
            objetoNotificavel.Notificacoes.First().Campo.Should().Be(nomeCampo);
            objetoNotificavel.Notificacoes.First().Mensagem.Should().Be(mensagem);
        }

        [Fact(DisplayName = nameof(NumeroMaximoCaracteres_QuandoValorNaoExcedeQuantidadeCaracteres_NaoDeveNotificarObjeto))]
        [Trait("Dominio", "ValidacoesDominio - Validacoes")]
        public void NumeroMaximoCaracteres_QuandoValorNaoExcedeQuantidadeCaracteres_NaoDeveNotificarObjeto()
        {
            //Arrange
            var texto = _fixture.Faker.Lorem.Text();

            var numeroMaximoCaracteres = 500;
            var mensagem = "A quantidade de caracteres foi excedida";
            var nomeCampo = "MotivoCancelamento";
            NotifiableObject objetoNotificavel = new NotaFake();

            //Act
            ValidacoesDominio.NumeroMaximoCaracteres(texto, numeroMaximoCaracteres, objetoNotificavel, nomeCampo, mensagem);

            //Assert
            objetoNotificavel.Notificacoes.Should().BeEmpty();
            objetoNotificavel.Notificacoes.Should().HaveCount(default(int));
        }

        [Fact(DisplayName = nameof(Validar_QuandoValidacaoFalaha_DeveNotificarObjeto))]
        public void Validar_QuandoValidacaoFalaha_DeveNotificarObjeto()
        {
            //Arrange
            var objetoNotificavel = new NotaFake(-1, -1, -1, -1);
            var validador  = NotaFakeValidador.Instance;

            //Act
            ValidacoesDominio.Validar(objetoNotificavel, validador);
            
            //Asssert
            objetoNotificavel.Notificacoes.Should().NotBeEmpty();
            objetoNotificavel.Notificacoes.Should().HaveCount(4);
            objetoNotificavel.Notificacoes.Select(x => x.Mensagem)
                .Should().Contain(ConstantesDominio.MensagensValidacoes.ERRO_USUARIO_INVALIDO);
            objetoNotificavel.Notificacoes.Select(x => x.Mensagem)
                .Should().Contain(ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO);
            objetoNotificavel.Notificacoes.Select(x => x.Mensagem)
                .Should().Contain(ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDO);
            objetoNotificavel.Notificacoes.Select(x => x.Mensagem)
                .Should().Contain(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
            objetoNotificavel.EhValida.Should().BeFalse();  
        }

        [Fact(DisplayName = nameof(Validar_QuandoValidacaoPassa_NaoDeveNotificarObjeto))]
        public void Validar_QuandoValidacaoPassa_NaoDeveNotificarObjeto()
        {
            //Arrange
            var objetoNotificavel = new NotaFake(1, 1, 1, 1);
            var validador  = NotaFakeValidador.Instance;

            //Act
            ValidacoesDominio.Validar(objetoNotificavel, validador);
            
            //Asssert
            objetoNotificavel.Notificacoes.Should().BeEmpty();            
            objetoNotificavel.EhValida.Should().BeTrue();
        }
    }
}