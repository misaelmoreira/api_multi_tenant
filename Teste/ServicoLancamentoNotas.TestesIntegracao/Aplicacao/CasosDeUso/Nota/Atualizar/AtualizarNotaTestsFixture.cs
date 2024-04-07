using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.TestesIntegracao.Base;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Atualizar;

[CollectionDefinition(nameof(AtualizarNotasTestsFixture))]
public class AtualizarTestsFixtureCollection
    : ICollectionFixture<AtualizarNotasTestsFixture>
{ }

public class AtualizarNotasTestsFixture
    :  BaseFixture
{
    public AtualizarNotaInput RetornaInput(int? alunoId =null,  int? atividadeId = null, double?valorNota = null)
        => new (alunoId ?? RetornaNumeroIdRandomico(), atividadeId ?? RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), valorNota ?? RetornaValorNotaAleatorioValido());

    public ServicoLancamentoNotaDbContext CriarDbContext()
    {
        var dbContext = new ServicoLancamentoNotaDbContext(
            new DbContextOptionsBuilder<ServicoLancamentoNotaDbContext>()
                .UseInMemoryDatabase("integration-tests-atualizar-nota")
                .Options
        );

        return dbContext;
    }

}