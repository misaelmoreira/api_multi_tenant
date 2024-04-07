namespace ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepositorio
{
    public interface IBuscaRepository<TAgregado>
        where TAgregado : IRaizAgregacao
    {    
        Task<BuscaOuput<TAgregado>> Buscar(BuscaInput input, CancellationToken cancellationToken);  
    }
}