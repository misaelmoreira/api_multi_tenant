using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.TestesIntegracao.Base;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Lancar;

[CollectionDefinition(nameof(LancarNotasTestsFixture))]
public class LancarTestsFixtureCollection
    : ICollectionFixture<LancarNotasTestsFixture>
{ }

public class LancarNotasTestsFixture
    :  BaseFixture
{
    public LancarNotaInput DevolveNotaInputValido(int? alunoId = null,  int? atividadeId = null, double?valorNota = null)
        => new (alunoId ?? RetornaNumeroIdRandomico(), atividadeId ?? RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), valorNota ?? RetornaValorNotaAleatorioValido(), false);

    public LancarNotaInput DevolveNotaSubstitutivaInputValido(int alunoId,  int atividadeId)
        => new (alunoId, atividadeId, RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), true);

    public LancarNotaInput DevolveNotaInputInvalido()
            => new(-1, -1, -1, 11, false);  

    public ServicoLancamentoNotaDbContext CriarDbContext()
    {
        var dbContext = new ServicoLancamentoNotaDbContext(
            new DbContextOptionsBuilder<ServicoLancamentoNotaDbContext>()
                .UseInMemoryDatabase("integration-tests-lancar-nota")
                .Options
        );

        return dbContext;
    }

}