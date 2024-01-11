using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicoLancamentoNotas.Dominio.Enums;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum
{
    public record struct NotaOutputModel(int AlunoId, int AtividadeId, double ValorNota, DateTime  DataLancamento, bool Cancelada, string MotivoCancelamento, StatusIntegracao StatusIntegracao);
}