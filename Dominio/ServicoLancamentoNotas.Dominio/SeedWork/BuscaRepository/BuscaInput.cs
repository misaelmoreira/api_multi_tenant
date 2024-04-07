using ServicoLancamentoNotas.Dominio.Enums;

namespace ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepositorio
{
    public record struct BuscaInput(int Pagina, int PorPagina, int? AlunoId, int? AtividadeId, string OrdenarPor, OrdenacaoBusca Ordenacao = OrdenacaoBusca.Asc);
}