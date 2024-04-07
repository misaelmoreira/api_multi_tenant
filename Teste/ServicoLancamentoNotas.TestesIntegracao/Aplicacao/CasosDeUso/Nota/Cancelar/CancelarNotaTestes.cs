using Bogus.DataSets;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Enums;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.Infra.Data.Repositories;
using ServicoLancamentoNotas.Infra.Data.UoW;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Cancelar;

[Collection(nameof(CancelarNotasTestsFixture))]
public class CancelarNotaTests 
{
    private readonly CancelarNotasTestsFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<CancelarNota> _logger;
    private readonly ICancelarNota _sut;
    private readonly ServicoLancamentoNotaDbContext _context;

    public CancelarNotaTests(CancelarNotasTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();  
        _unitOfWork = new UnitOfWork(_context);
        _notaRepository = new NotaRepository(_context);
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<CancelarNota>();
        _sut = new CancelarNota(_notaRepository, _unitOfWork, _logger);        
        
    }

    [Fact(DisplayName = nameof(Handle_QuandoCancelarInput_DeveCancelarNota))]
    [Trait("Aplicacao", "Integracao/CancelarNota - Casos de Uso")]
    public async Task Handle_QuandoCancelarInput_DeveCancelarNota()
    {
        // arrange
        var nota = _fixture.RetornaNota();
        var motivoCancelamento = "Lancamento indevido";
        var input = _fixture.RetornaInput(nota.AlunoId, nota.AtividadeId, motivoCancelamento);
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        //act
        var resposta = await _sut.Handle(input, CancellationToken.None);

        //Assert
        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeTrue(); 
        resposta.Dado.MotivoCancelamento.Should().Be(motivoCancelamento);   
        resposta.Dado.Cancelada.Should().BeTrue();

        var notaSalva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == nota.AlunoId && x.AtividadeId == nota.AtividadeId);

        notaSalva.Should().NotBeNull();
        notaSalva!.MotivoCancelamento.Should().Be(motivoCancelamento);
        notaSalva!.DataAtualizacao.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(Handler_QuandoNotaNaoEncontrada_DeveRetornarErroNotaNaoEncotrada))]
    [Trait("Aplicacao", "Integracao/CancelarNota - Casos de Uso")]
    public async Task Handler_QuandoNotaNaoEncontrada_DeveRetornarErroNotaNaoEncotrada()
    {
        // arrange
        var motivoCancelamento = "Lancamento indevido";
        var input = _fixture.RetornaInput(motivo: motivoCancelamento);

        //act
        var resposta = await _sut.Handle(input, CancellationToken.None);

        //Assert
        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeFalse(); 
        resposta.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaNaoEncontrada);
    }

    [Fact(DisplayName = nameof(Handler_QuandoNotaInputInvalido_DeveRetornarErroNota))]
    [Trait("Aplicacao", "Integracao/CancelarNota - Casos de Uso")]
    public async Task Handler_QuandoNotaInputInvalido_DeveRetornarErroNota()
    {
        // arrange
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput(nota.AlunoId, nota.AtividadeId);
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        //act
        var resposta = await _sut.Handle(input, CancellationToken.None);

        //Assert
        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeFalse(); 
        resposta.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaInvalida);
        resposta.DetalhesErros.Should().NotBeEmpty();
        resposta.DetalhesErros.Should().HaveCount(1);
        resposta.DetalhesErros.FirstOrDefault().Should().NotBeNull();
        resposta.DetalhesErros.FirstOrDefault()!.Campo.Should().Be("MotivoCancelamento");
    }
}