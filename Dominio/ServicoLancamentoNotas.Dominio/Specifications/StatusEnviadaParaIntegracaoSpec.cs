using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dominio.ServicoLancamentoNotas.Dominio.Entidades;
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