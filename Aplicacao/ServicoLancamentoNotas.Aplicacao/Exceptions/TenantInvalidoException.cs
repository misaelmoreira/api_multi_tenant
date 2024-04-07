
using ServicoLancamentoNotas.Dominio.ObjetosValor;

namespace ServicoLancamentoNotas.Aplicacao.Exceptions
{
    public class TenantInvalidoException : ApplicationException
    {
        public TenantInvalidoException(Tenant tenant) : base($"O tenant {tenant} não é válido") {}
                
    }
}