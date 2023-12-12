using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicoLancamentoNotas.Dominio.Constantes;

public static class ConstantesDominio
{
    public static class MensagensValidacoes
    {
        public const string ERRO_VALOR_NOTA_INVALIDO = "O valor da nota deve estar no intervalo de 0 a 10";
        public const string ERRO_USUARIO_INVALIDO = "O identificador do usuario deve ser maior que 0";
        public const string ERRO_ALUNO_INVALIDO = "O identificador do aluno deve ser maior que 0";
        public const string ERRO_ATIVIDADE_INVALIDO = "O identificador da Atividade deve ser maior que 0";
    } 
}
