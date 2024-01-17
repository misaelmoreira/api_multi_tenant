using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

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