using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.Comum;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs
{
    public record struct LancarNotaInput(int AlunoId, int AtividadeId, int ProfessorId, double ValorNota, bool NotaSubstitutiva) : IRequest<Resultado<NotaOutputModel>>;
}