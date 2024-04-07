using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.Comum;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.Interfaces
{
    public interface ICancelarNota : IRequestHandler<CancelarNotaInput, Resultado<NotaOutputModel>>
    {
        
    }
}