using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Mediator;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.Infra.Data.Providers;
using ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;
using ServicoLancamentoNotas.Infra.Data.Repositories;
using ServicoLancamentoNotas.Infra.Data.UoW;

namespace ServicoLancamentoNotas.Infra.CrossCutting.IoC
{
    public static class BootStrapper
    {
        public static void RegistrarServicos(this IServiceCollection services)
        {
            services
                .RegistrarRepositorios()
                .RegistrarDbContext()
                .RegistrarUnitOfWork()                
                .RegistrarHandlers()
                .RegistrarMediator()
                .RegistrarProviders();
        }
        
        private static IServiceCollection RegistrarRepositorios(this IServiceCollection services)
            => services.AddScoped<INotaRepository, NotaRepository>();

        private static IServiceCollection RegistrarDbContext(this IServiceCollection services)
            => services.AddDbContext<ServicoLancamentoNotaDbContext>(options => 
            {
                options.UseInMemoryDatabase("db-teste-in-memory");
            });

        private static IServiceCollection RegistrarHandlers(this IServiceCollection services)
            => services.AddMediatR(typeof(ConsultaNota));

        private static IServiceCollection RegistrarUnitOfWork(this IServiceCollection services)
            => services.AddScoped<IUnitOfWork, UnitOfWork>();

        private static IServiceCollection RegistrarMediator(this IServiceCollection services)
            => services.AddScoped<IMediatorHandler, MediatorHandler>();

        private static IServiceCollection RegistrarProviders(this IServiceCollection services)
            => services.AddScoped<ITenantProvider, TenantProvider>()
                        .AddScoped<IVariaveisAmbienteProvider, VariaveisAmbienteProvider>();
    }
}