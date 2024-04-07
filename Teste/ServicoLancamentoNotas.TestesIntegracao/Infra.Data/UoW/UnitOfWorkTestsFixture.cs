using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepositorio;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.TestesIntegracao.Base;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Infra.Data.Repositories;

[CollectionDefinition(nameof(UnitOfWorkTestsFixture))]
public class UnitOfWorkTestsFixtureCollection
    : ICollectionFixture<UnitOfWorkTestsFixture> {}

public class UnitOfWorkTestsFixture 
    : BaseFixture
{
    public BuscaInput RetornarBuscaInputApenasComPaginacao(int? alunoId = null, int? atividadeId = null, OrdenacaoBusca ordenacao = OrdenacaoBusca.Asc, string ordenarPor = "", int? pagina = null, int? porPagina = null)
        => new(pagina ?? 1, porPagina ?? 10, alunoId ?? null, atividadeId ?? null, ordenarPor, ordenacao);    

    public List<Nota> RetornarNotas(int? quantidadeGerada = null)
        => Enumerable.Range(1, quantidadeGerada ?? 10).Select(id => RetornaNota(id)).ToList();

    public ServicoLancamentoNotaDbContext CriarDbContext()
    {
        var dbContext = new ServicoLancamentoNotaDbContext(
            new DbContextOptionsBuilder<ServicoLancamentoNotaDbContext>()
                .UseInMemoryDatabase("integration-tests-unit-of-work")
                .Options
        );

        return dbContext;
    }
    
}