using System;
using System.Collections.Generic;
using System.Linq;
using Dominio.ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLancamentoNotas.Dominio.Constantes;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Exceptions;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Dominio.Entidades
{
    public class NotaTestes
    {
        [Fact(DisplayName = "")]
        [Trait("Dominio", "Nota - Agregado")]
        public void InstanciarNota()
        {
            //Arrange
            var valoresEntrada = new 
            { 
                AlunoId = 1234, 
                AtividadeId = 34566, 
                ValorNota = 10.00, 
                DataLancamento = DateTime.Now,
                UsuarioId = 34566,
                DataAtualizacao = DateTime.Now
            };


            //Act
            var nota = new Nota(valoresEntrada.AlunoId, valoresEntrada.AtividadeId, valoresEntrada.ValorNota, valoresEntrada.DataLancamento, valoresEntrada.UsuarioId);

            //Assert
            Assert.NotNull(nota);
            Assert.Equal(valoresEntrada.AlunoId, nota.AlunoId);
            Assert.Equal(valoresEntrada.AtividadeId, nota.AtividadeId);
            Assert.Equal(valoresEntrada.ValorNota, nota.ValorNota);
            Assert.Equal(valoresEntrada.DataLancamento, nota.DataLancamento);
            Assert.NotEqual(default(DateTime), nota.DataLancamento);
            Assert.NotEqual(default(DateTime), nota.DataAtualizacao);
            Assert.Equal(valoresEntrada.UsuarioId, nota.UsuarioId);
            Assert.False(nota.CanceladaPorRetentativa);
            Assert.Equal(StatusIntegracao.AguardandoIntegracao, nota.StatusIntegracao);
            Assert.Null(nota.MotivoCancelamento);

        }

        
        [Theory(DisplayName = nameof(InstanciarNota_QuandoValorNotaInvalido_DeveLancarExcessao))]
        [Trait("Dominio", "Nota - Agregado")]
        [InlineData(-1)]
        [InlineData(11)]
        public void InstanciarNota_QuandoValorNotaInvalido_DeveLancarExcessao(double valorNota)
        {
            //Arrange
            var valoresEntrada = new 
            { 
                AlunoId = 1234, 
                AtividadeId = 34566, 
                ValorNota = valorNota, 
                DataLancamento = DateTime.Now,
                UsuarioId = 34566,
                DataAtualizacao = DateTime.Now
            };

            //Act
            var action = () => new Nota(valoresEntrada.AlunoId, valoresEntrada.AtividadeId, valoresEntrada.ValorNota, valoresEntrada.DataLancamento, valoresEntrada.UsuarioId);

            
            //Assert
            var exception = Assert.Throws<ValidacaoEntidadeException>(action);
            Assert.NotNull(exception);
            Assert.Equal(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO, exception.Message);
        }


        [Theory(DisplayName = nameof(InstanciarNota_QuandoUsuarioIdInvalido_DeveLancarExcessao))]
        [Trait("Dominio", "Nota - Agregado")]
        [InlineData(-1)]
        [InlineData(0)]
        public void InstanciarNota_QuandoUsuarioIdInvalido_DeveLancarExcessao(int usuarioId)
        {
            //Arrange
            var valoresEntrada = new 
            { 
                AlunoId = 1234, 
                AtividadeId = 34566, 
                ValorNota = 10, 
                DataLancamento = DateTime.Now,
                UsuarioId = usuarioId,
                DataAtualizacao = DateTime.Now
            };

            //Act
            var action = () => new Nota(valoresEntrada.AlunoId, valoresEntrada.AtividadeId, valoresEntrada.ValorNota, valoresEntrada.DataLancamento, valoresEntrada.UsuarioId);

            
            //Assert
            var exception = Assert.Throws<ValidacaoEntidadeException>(action);
            Assert.NotNull(exception);
            Assert.Equal(ConstantesDominio.MensagensValidacoes.ERRO_USUARIO_INVALIDO, exception.Message);
        }

        [Theory(DisplayName = nameof(InstanciarNota_QuandoAlunoIdInvalido_DeveLancarExcessao))]
        [Trait("Dominio", "Nota - Agregado")]
        [InlineData(-1)]
        [InlineData(0)]
        public void InstanciarNota_QuandoAlunoIdInvalido_DeveLancarExcessao(int alunoId)
        {
            //Arrange
            var valoresEntrada = new 
            { 
                AlunoId = alunoId, 
                AtividadeId = 34566, 
                ValorNota = 10, 
                DataLancamento = DateTime.Now,
                UsuarioId = 1,
                DataAtualizacao = DateTime.Now
            };

            //Act
            var action = () => new Nota(valoresEntrada.AlunoId, valoresEntrada.AtividadeId, valoresEntrada.ValorNota, valoresEntrada.DataLancamento, valoresEntrada.UsuarioId);

            
            //Assert
            var exception = Assert.Throws<ValidacaoEntidadeException>(action);
            Assert.NotNull(exception);
            Assert.Equal(ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO, exception.Message);
        }

        [Theory(DisplayName = nameof(InstanciarNota_QuandoAtividadeIdInvalido_DeveLancarExcessao))]
        [Trait("Dominio", "Nota - Agregado")]
        [InlineData(-1)]
        [InlineData(0)]
        public void InstanciarNota_QuandoAtividadeIdInvalido_DeveLancarExcessao(int atividadeId)
        {
            //Arrange
            var valoresEntrada = new 
            { 
                AlunoId = 1, 
                AtividadeId = atividadeId, 
                ValorNota = 10, 
                DataLancamento = DateTime.Now,
                UsuarioId = 1,
                DataAtualizacao = DateTime.Now
            };

            //Act
            var action = () => new Nota(valoresEntrada.AlunoId, valoresEntrada.AtividadeId, valoresEntrada.ValorNota, valoresEntrada.DataLancamento, valoresEntrada.UsuarioId);

            
            //Assert
            var exception = Assert.Throws<ValidacaoEntidadeException>(action);
            Assert.NotNull(exception);
            Assert.Equal(ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDO, exception.Message);
        }

        //Controle de nota lancada já foi integrada
        //Caso uma nota venha a ser cancelada preciso de um motivo para o cancelamento
        //Uma valor de nota tem 

    }
}