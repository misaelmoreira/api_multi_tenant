using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs
{
    public record struct CancelarNotaInput(int AlunoId, int AtividadeId, int ProfessorId, string Motivo) : IRequest<NotaOutputModel>;
}