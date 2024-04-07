using ServicoLancamentoNotas.Dominio.Enums;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum
{
    public record class NotaOutputModel(int AlunoId, int AtividadeId, double ValorNota, DateTime  DataLancamento, bool Cancelada, string MotivoCancelamento, StatusIntegracao StatusIntegracao);
}