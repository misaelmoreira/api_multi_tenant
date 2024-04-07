using ServicoLancamentoNotas.Dominio.SeedWork;

namespace ServicoLacamentoNotas.Testes
{
    public class NotaFake : NotifiableObject
    {
        public NotaFake() {}

        public NotaFake(int alunoId, int atividadeId, double valorNota, int usuarioId, bool canceladaPorRetentativa = false, string? motivoCancelamento = null)
        {
            AlunoId = alunoId;
            AtividadeId = atividadeId;
            ValorNota = valorNota;
            UsuarioId = usuarioId;
            CanceladaPorRetentativa = canceladaPorRetentativa;
            MotivoCancelamento = motivoCancelamento;
        }

        public int AlunoId { get; private set; }
        public int AtividadeId { get; private set; }
        public double ValorNota { get; private set; }
        public int UsuarioId { get; private set; }        
        public bool CanceladaPorRetentativa { get; private set; }
        public string? MotivoCancelamento { get; private set; }
    }
}