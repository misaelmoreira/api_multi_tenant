using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Dominio.Entidades;

namespace ServicoLancamentoNotas.Infra.Data.Contexto;

public class ServicoLancamentoNotaDbContext : DbContext
{
    public DbSet<Nota> Notas => Set<Nota>();

    public ServicoLancamentoNotaDbContext(DbContextOptions<ServicoLancamentoNotaDbContext> options ) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}