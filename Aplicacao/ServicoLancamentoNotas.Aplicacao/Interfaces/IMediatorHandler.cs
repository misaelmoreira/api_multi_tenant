using MediatR;

namespace ServicoLancamentoNotas.Aplicacao.Interfaces
{
    public interface IMediatorHandler
    {    
        Task<TResponse> EnviarRequest<TResponse, TRequest>(TRequest request, CancellationToken cancellationToken)
            where TRequest : IRequest<TResponse>;
    }
}