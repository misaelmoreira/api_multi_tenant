using ServicoLancamentoNotas.Infra.CrossCutting.IoC;

namespace api.Configurations;
public static class ConfigurationInjecaoDependenciaExtension
{
    public static IServiceCollection ConfigurarServicos(this IServiceCollection services)
    {
        services.AddControllers();
        services.RegistrarServicos();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddSwaggerGen();
        return services;
    }
}