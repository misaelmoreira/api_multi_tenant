using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Aplicacao.Comum;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces
{
    public interface ILancarNota : IRequestHandler<LancarNotaInput, Resultado<NotaOutputModel>>
    {
        
    }
}