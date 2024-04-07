
namespace ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepositorio
{
    public record struct BuscaOuput<TAgregado>
        where TAgregado : IRaizAgregacao
    {
        public BuscaOuput(int pagina, int porPagina, int total, IReadOnlyList<TAgregado> items)
        {
            Pagina = pagina;
            PorPagina = porPagina;
            Total = total;
            Items = items;
        }

        public int Pagina { get; set; }
        public int PorPagina { get; set; }
        public int Total { get; set; }
        public IReadOnlyList<TAgregado> Items { get; set; }
    }
}