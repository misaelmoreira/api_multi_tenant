using ServicoLancamentoNotas.Dominio.Constantes;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Params;
using ServicoLancamentoNotas.Dominio.SeedWork;
using ServicoLancamentoNotas.Dominio.Validacoes;
using ServicoLancamentoNotas.Dominio.Validacoes.Validador;

namespace ServicoLancamentoNotas.Dominio.Entidades;

public partial class Nota : Entidade, IRaizAgregacao
{
    private const double VALOR_MAXIMO_NOTA = 10.00;

    public int AlunoId { get; private set; }
    public int AtividadeId { get; private set; }
    public double ValorNota { get; private set; }
    public DateTime DataLancamento { get; private set; }
    public int UsuarioId { get; private set; }
    public bool CanceladaPorRetentativa { get; private set; }
    public bool Cancelada { get; private set; }
    public string? MotivoCancelamento { get; private set; }
    public StatusIntegracao StatusIntegracao { get; private set; }

    // Construtor sem parâmetros (necessário para o Entity Framework)
    protected Nota() {}

    public Nota(NotaParams notaParams)
    {
        AlunoId = notaParams.AlunoId;
        AtividadeId = notaParams.AtividadeId;
        ValorNota = notaParams.ValorNota;
        DataLancamento = notaParams.DataLancamento;
        UsuarioId = notaParams.UsuarioId;
        CanceladaPorRetentativa = false;
        DataCriacao = DateTime.Now;
        StatusIntegracao = notaParams.StatusIntegracao;
        Validar();
    }

    private void Validar()
    {
        ValidacoesDominio
            .Validar(this, NotaValidador.Instance);                
    }

    public void Cancelar(string motivoCancelamento)
    {
        if(string.IsNullOrWhiteSpace(motivoCancelamento))
        {
            Notificar(new(nameof(MotivoCancelamento), ConstantesDominio.MensagensValidacoes.ERRO_MOTIVO_CANCELAMENTO_NAO_INFORMADO));
            EhValida = false;
            return;
        }
        MotivoCancelamento = motivoCancelamento;
        Cancelada = true;
        DataAtualizacao = DateTime.Now;
        Validar();
    }

    public void CancelarPorRetentativa()
    {
        MotivoCancelamento = ConstantesDominio.Mensagens.NOTA_CANCELADA_POR_RETENTATIVA;
        Cancelada = true;
        CanceladaPorRetentativa = true;
        DataAtualizacao = DateTime.Now;
        Validar();
    }
    
    public void AtualizarValorNota(double novoValorNota)
    {
        ValorNota = novoValorNota;
        DataAtualizacao = DateTime.Now;
        Validar(); 
    }

    public void AtualizarStatusIntegracao(StatusIntegracao novoStatus)
    {
        if(StatusIntegracao  == StatusIntegracao.AguardandoIntegracao
            && novoStatus == StatusIntegracao.IntegradaComSucesso
            || novoStatus == StatusIntegracao.FalhaNaIntegracao)
            {
                Notificar(new Notificacao(nameof(StatusIntegracao), $"Não é permitida a mudança do status {StatusIntegracao} para {novoStatus}"));

                EhValida = false;
                return;
            }

        StatusIntegracao = novoStatus;
        DataAtualizacao = DateTime.Now;
        Validar();
    }

    public void AlterarStatusIntegracaoParaEnviada()
    {
        ValidarStatus(PodeAlterarStatusParaEnviado, StatusIntegracao.EviadaParaIntegracao);
        if(Notificacoes.Any())
        {
            EhValida = false;
            return;
        }
        StatusIntegracao = StatusIntegracao.EviadaParaIntegracao;
        DataAtualizacao = DateTime.Now;
        Validar();
    }
    
    public void AlterarStatusIntegracaoParaFalhaIntegracao()
    {
        ValidarStatus(PodeAlterarStatusParaFalhaIntegracao, StatusIntegracao.FalhaNaIntegracao);
        if(Notificacoes.Any())
        {
            EhValida = false;
            return;
        }
        StatusIntegracao = StatusIntegracao.FalhaNaIntegracao;
        DataAtualizacao = DateTime.Now;
        Validar();
    }
    
    public void AlterarStatusParaIntegradaComSucesso()
    {
        ValidarStatus(PodeAlterarStatusParaIntegradaComSucesso, StatusIntegracao.IntegradaComSucesso);
        if(Notificacoes.Any())
        {
            EhValida = false;
            return;
        }
        StatusIntegracao = StatusIntegracao.IntegradaComSucesso;
        DataAtualizacao = DateTime.Now;
        Validar();
    }

    private void ValidarStatus(Func<bool> podeAlterarStatus, StatusIntegracao proximoStatus)
    {
        if(!podeAlterarStatus())
            Notificar(new(nameof(StatusIntegracao), string.Format(ConstantesDominio.Mensagens.ALTERACAO_DE_STATUS_NAO_PERMITIDA, proximoStatus.ToString())));
    }    
}