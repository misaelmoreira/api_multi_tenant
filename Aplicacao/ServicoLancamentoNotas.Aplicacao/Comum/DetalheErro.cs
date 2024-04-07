namespace ServicoLancamentoNotas.Aplicacao.Comum;

public sealed class DetalheErro
{
    public DetalheErro(string campo, string mensagem)
    {
        Campo = campo;
        Mensagem = mensagem;
    }

    public string Campo { get; private set; }
    public string Mensagem { get; private set; }
}