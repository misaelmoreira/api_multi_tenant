using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLancamentoNotas.Dominio.SeedWork;
using ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepositorio;

namespace ServicoLancamentoNotas.Dominio.Repositories
{
    public interface INotaRepository : IGenericRepository<Nota>, IBuscaRepository<Nota>
    {
        Task<Nota?> BuscarNotaPorAlunoEAtividade(int alunoId, int atividadeId, CancellationToken cancellationToken);            
    }
}