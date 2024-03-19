using System.ComponentModel;
using ServicoLancamentoNotas.Aplicacao.Constantes;

namespace ServicoLancamentoNotas.Aplicacao.Enums;

public enum TipoErro
{
    [Description(ConstantesAplicacao.MensagemErro.NOTA_NAO_ENCONTRADA)]
    NotaNaoEncontrada = 100,

    [Description(ConstantesAplicacao.MensagemErro.NOTA_INVALIDA)]
    NotaInvalida = 101,

    [Description(ConstantesAplicacao.MensagemErro.ERRO_INESPERDO)]
    ErroInesperado = 500
}