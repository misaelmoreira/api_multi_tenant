using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServicoLancamentoNotas.Dominio.Specifications.Base
{
    [ExcludeFromCodeCoverage]
    public abstract class Specification<T> : ISpecification<T>
    {
        public bool IsSatisfied(T obj)
        {
            return ToExpression().Compile().Invoke(obj);
        }

        public abstract Expression<Func<T, bool>> ToExpression();
    }
}