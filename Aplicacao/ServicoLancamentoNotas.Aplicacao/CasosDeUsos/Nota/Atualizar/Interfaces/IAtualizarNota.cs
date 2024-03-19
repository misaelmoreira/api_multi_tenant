using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.Comum;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces
{
    public interface IAtualizarNota : IRequestHandler<AtualizarNotaInput, Resultado<NotaOutputModel>>
    {
        
    }
}