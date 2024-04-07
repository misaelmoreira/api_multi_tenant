using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.Infra.Data.UoW;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Infra.Data.Repositories;

[Collection(nameof(UnitOfWorkTestsFixture))]
public class UnitOfWorkTests
{
    private readonly IUnitOfWork _sut;
    private readonly UnitOfWorkTestsFixture _fixture;
    private ServicoLancamentoNotaDbContext _context;

    public UnitOfWorkTests(UnitOfWorkTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _sut = new UnitOfWork(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }    

    [Fact(DisplayName = nameof(Rollback_QuandoInvocado_NaoDeveLancarExcecao))]
    [Trait("Infra.Data", "Integracao/Uow - Unit Of Work")]
    public async Task Rollback_QuandoInvocado_NaoDeveLancarExcecao()
    {
        //arrange

        //act        

        //assert 
        await _sut.Awaiting(x => x.Rollback(CancellationToken.None))
                .Should().NotThrowAsync();
    }

    
    [Fact(DisplayName = nameof(Commit_QuandoRealizadoPersistencia_DeveRetornarVerdadeiro))]
    [Trait("Infra.Data", "Integracao/Uow - Unit Of Work")]
    public async Task Commit_QuandoRealizadoPersistencia_DeveRetornarVerdadeiro()
    {
        //arrange
        var notas = _fixture.RetornarNotas();
        await _context.AddRangeAsync(notas);

        //act
        var resultado = await _sut.Commit(CancellationToken.None);

        //assert 
        var notasSalvas = await _context.Notas.ToListAsync();
        resultado.Should().BeTrue();
        notasSalvas.Should().NotBeEmpty();
    }

    [Fact(DisplayName = nameof(Commit_QuandoNaoExisteNotasParaPersistencia_DeveRetornarFalso))]
    [Trait("Infra.Data", "Integracao/Uow - Unit Of Work")]
    public async Task Commit_QuandoNaoExisteNotasParaPersistencia_DeveRetornarFalso()
    {
        //arrange

        //act
        var resultado = await _sut.Commit(CancellationToken.None);

        //assert 
        resultado.Should().BeFalse();
    }
    
}