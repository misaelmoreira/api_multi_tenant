using System.Net.Mime;
using System.Text.Json;
using ServicoLancamentoNotas.Aplicacao.Exceptions;
using ServicoLancamentoNotas.Infra.Data.Providers.Interfaces;

namespace ServicoLancamentoNotas.Api.Controllers.Middlewares
{
    public class BuscaTenantMiddleware : IMiddleware
    {
        private readonly ITenantProvider _tenantProvider;

        public BuscaTenantMiddleware(ITenantProvider tenantProvider)
            => _tenantProvider = tenantProvider;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var tenant = _tenantProvider.ObterTenant();

            if (_tenantProvider.ValidarTenant(tenant))
            {
                _tenantProvider.AtribuirTenant(tenant);
                await next(context);
            }
            else
            {
                await DevolverExcecaoTenantInvalido(context, new TenantInvalidoException(tenant));
            }
        }

        private Task DevolverExcecaoTenantInvalido(HttpContext context, TenantInvalidoException exception)
        {
            var logger = context.RequestServices.GetService<ILogger<BuscaTenantMiddleware>>();
            logger?.LogError("Ocorreu um erro ao setar o tenant.{@exception}", exception);

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var json = JsonSerializer.Serialize(new 
            {
                correlationId = Guid.NewGuid(),
                context.Response.StatusCode,
                Detalhes = new 
                {
                    Tipo = exception.GetType().ToString(),
                    Mensagem = exception.Message,
                    exception.StackTrace
                }
            });

            return context.Response.WriteAsync(json);
        }
    }
}