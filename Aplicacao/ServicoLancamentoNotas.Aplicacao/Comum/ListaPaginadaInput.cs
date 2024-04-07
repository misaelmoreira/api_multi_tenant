using ServicoLancamentoNotas.Dominio.Enums;

namespace ServicoLancamentoNotas.Aplicacao.Comum;

public abstract record class ListaPaginadaInput
    (int Pagina, int PorPagina, int? AlunoId, int? AtividadeId, string OrdenarPor, OrdenacaoBusca Ordenacao);