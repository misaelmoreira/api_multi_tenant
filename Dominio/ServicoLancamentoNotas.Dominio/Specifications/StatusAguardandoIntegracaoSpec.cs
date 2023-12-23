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
    public class StatusAguardandoIntegracaoSpec : Specification<Nota>
    {
        public static readonly StatusAguardandoIntegracaoSpec Instance = new();

        public override Expression<Func<Nota, bool>> ToExpression()
            => nota => nota.StatusIntegracao == StatusIntegracao.AguardandoIntegracao;
    }
}