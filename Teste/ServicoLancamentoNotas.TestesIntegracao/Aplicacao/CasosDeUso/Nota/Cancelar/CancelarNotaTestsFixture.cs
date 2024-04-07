using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.TestesIntegracao.Base;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Cancelar;

[CollectionDefinition(nameof(CancelarNotasTestsFixture))]
public class CancelarTestsFixtureCollection
    : ICollectionFixture<CancelarNotasTestsFixture>
{ }

public class CancelarNotasTestsFixture
    :  BaseFixture
{
    public CancelarNotaInput RetornaInput(int? alunoId =null,  int? atividadeId = null, string?motivo = null)
        => new (alunoId ?? RetornaNumeroIdRandomico(), atividadeId ?? RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), motivo ?? string.Empty);

    public ServicoLancamentoNotaDbContext CriarDbContext()
    {
        var dbContext = new ServicoLancamentoNotaDbContext(
            new DbContextOptionsBuilder<ServicoLancamentoNotaDbContext>()
                .UseInMemoryDatabase("integration-tests-cancelar-nota")
                .Options
        );

        return dbContext;
    }

}