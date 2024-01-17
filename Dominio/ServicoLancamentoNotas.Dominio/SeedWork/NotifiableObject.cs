namespace ServicoLancamentoNotas.Dominio.SeedWork
{
    public abstract class NotifiableObject
    {
        public bool EhValida { get; set; }
        public List<Notificacao> Notificacoes { get; } = new();

        public void Notificar(Notificacao notificacao)
            => Notificacoes.Add(notificacao);
    }
}