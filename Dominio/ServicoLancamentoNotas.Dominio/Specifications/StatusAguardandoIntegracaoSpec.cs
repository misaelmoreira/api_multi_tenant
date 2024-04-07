using System.Linq.Expressions;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Specifications.Base;

namespace ServicoLancamentoNotas.Dominio.Specifications
{
    public class StatusAguardandoIntegracaoSpec : Specification<Nota>
    {
        public static readonly StatusAguardandoIntegracaoSpec Instance = new();

        public override Expression<Func<Nota, bool>> ToExpression()
            => nota => nota.StatusIntegracao == StatusIntegracao.AguardandoIntegracao;
    }
}