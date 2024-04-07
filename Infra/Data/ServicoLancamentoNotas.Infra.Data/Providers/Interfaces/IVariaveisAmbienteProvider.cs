using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicoLancamentoNotas.Infra.Data.Providers.Interfaces
{
    public interface IVariaveisAmbienteProvider
    {
        HashSet<string> Tenants { get; }       
    }
}