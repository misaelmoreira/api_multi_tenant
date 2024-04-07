using ServicoLancamentoNotas.Dominio.ObjetosValor;

namespace ServicoLancamentoNotas.Infra.Data.Providers.Interfaces
{
    public interface ITenantProvider
    {
        Tenant ObterTenant();
        void AtribuirTenant(Tenant tenant);
        bool ValidarTenant(Tenant tenant);        
    }
}