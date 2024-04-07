using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Infra.Data.Contexto;

namespace ServicoLancamentoNotas.Infra.Data.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ServicoLancamentoNotaDbContext _context;

    public UnitOfWork(ServicoLancamentoNotaDbContext context)
        => _context = context;

    public async Task<bool> Commit(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken) > default(int);

    public Task Rollback(CancellationToken cancellationToken)
        => Task.CompletedTask;
}