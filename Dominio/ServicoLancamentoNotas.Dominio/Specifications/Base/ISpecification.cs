using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ServicoLancamentoNotas.Dominio.Specifications.Base
{
    
    public interface ISpecification<in T>
    {
        bool IsSatisfied(T obj);
        
    }
}