using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs
{
    public record struct AtualizarNotaInput(int AlunoId, int AtividadeId, int ProfessorId, double ValorNota) : IRequest<NotaOutputModel>;
}