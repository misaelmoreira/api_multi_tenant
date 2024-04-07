using System.Linq.Expressions;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Specifications.Base;

namespace ServicoLancamentoNotas.Dominio.Specifications
{
    public class StatusEnviadaParaIntegracaoSpec : Specification<Nota>
    {
        public static readonly StatusEnviadaParaIntegracaoSpec Instance = new();

        public override Expression<Func<Nota, bool>> ToExpression()
            => nota => nota.StatusIntegracao == StatusIntegracao.EviadaParaIntegracao;
    }
}