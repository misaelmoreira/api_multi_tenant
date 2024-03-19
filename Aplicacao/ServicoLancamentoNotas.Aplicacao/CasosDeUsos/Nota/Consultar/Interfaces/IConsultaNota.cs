using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Aplicacao.Comum;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces
{
    public interface IConsultaNota : IRequestHandler<ListaNotaInput, Resultado<ListaNotaOutput>>
    {
        
    }
}