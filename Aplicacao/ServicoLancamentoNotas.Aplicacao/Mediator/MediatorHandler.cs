using MediatR;
using ServicoLancamentoNotas.Aplicacao.Interfaces;

namespace ServicoLancamentoNotas.Aplicacao.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IMediator _mediator;

    public MediatorHandler(IMediator mediator)      
    {
        _mediator = mediator;
    }

    public async Task<TResponse> EnviarRequest<TResponse, TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest<TResponse>
        =>  await _mediator.Send(request, cancellationToken);
}