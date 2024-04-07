using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.Comum;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs
{
    public record struct AtualizarNotaInput(int AlunoId, int AtividadeId, int ProfessorId, double ValorNota) : IRequest<Resultado<NotaOutputModel>>;
}