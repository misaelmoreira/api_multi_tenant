using MediatR;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Dominio.Enums;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs
{
    public record class ListaNotaInput : ListaPaginadaInput, IRequest<ListaNotaOutput>
    {
        public ListaNotaInput(int Pagina, int PorPagina, int? AlunoId, int? AtividadeId, string OrdenarPor, OrdenacaoBusca OrdenacaoBusca = OrdenacaoBusca.Asc) : base(Pagina, PorPagina, AlunoId, AtividadeId, OrdenarPor, OrdenacaoBusca)
        {
        }
    }
}