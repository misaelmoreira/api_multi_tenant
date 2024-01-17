using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces
{
    public interface IConsultaNota : IRequestHandler<ListaNotaInput, ListaNotaOutput>
    {
        
    }
}