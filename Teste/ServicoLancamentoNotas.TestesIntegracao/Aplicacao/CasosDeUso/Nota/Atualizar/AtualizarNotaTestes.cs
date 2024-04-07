using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Enums;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.Infra.Data.Repositories;
using ServicoLancamentoNotas.Infra.Data.UoW;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Atualizar;

[Collection(nameof(AtualizarNotasTestsFixture))]
public class AtualizarNotaTests 
{
    private readonly AtualizarNotasTestsFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<AtualizarNota> _logger;
    private readonly IAtualizarNota _sut;
    private readonly ServicoLancamentoNotaDbContext _context;

    public AtualizarNotaTests(AtualizarNotasTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _unitOfWork = new UnitOfWork(_context);
        _notaRepository = new NotaRepository(_context);
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<AtualizarNota>();
        _sut = new AtualizarNota(_notaRepository, _unitOfWork, _logger);        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoNotaExiste_DeveAtualizarValores))]
    [Trait("Aplicacao", "Integracao/AtualizarNota - Casos de Uso")]
    public async Task Atualizar_QuandoNotaExiste_DeveAtualizarValores()
    {
        // arrange
        var novoValorNota = 10;
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput(nota.AlunoId, nota.AtividadeId, novoValorNota);
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        //act
        var resposta = await _sut.Handle(input, CancellationToken.None);

        //Assert
        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeTrue(); 
        resposta.Dado.ValorNota.Should().Be(novoValorNota);   

        var notaSalva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == nota.AlunoId && x.AtividadeId == nota.AtividadeId);

        notaSalva.Should().NotBeNull();
        notaSalva!.ValorNota.Should().Be(novoValorNota);
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoNotaNaoExiste_DeveInformarErroNotaNaoEncotrada))]
    [Trait("Aplicacao", "Integracao/AtualizarNota - Casos de Uso")]
    public async Task Atualizar_QuandoNotaNaoExiste_DeveInformarErroNotaNaoEncotrada()
    {
        // arrange
        var input = _fixture.RetornaInput();

        //act
        var resposta = await _sut.Handle(input, CancellationToken.None);

        //Assert
        resposta.Should().NotBeNull();
        resposta.Sucesso.Should().BeFalse(); 
        resposta.Dado.Should().BeNull();
        resposta.Erro.Should().Be(TipoErro.NotaNaoEncontrada);
    }

    [Theory(DisplayName = nameof(Atualizar_QuandoNotaInformadoInputInvalido_DeveInformarErroNotaInvalida))]
    [InlineData(-1)]
    [InlineData(11)]
    [Trait("Aplicacao", "Integracao/AtualizarNota - Casos de Uso")]
    public async Task Atualizar_QuandoNotaInformadoInputInvalido_DeveInformarErroNotaInvalida(int novoValorNota)
    {
        // arrange
        var nota = _fixture.RetornaNota();
        var input = _fixture.RetornaInput(nota.AlunoId, nota.AtividadeId, novoValorNota);
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
        resposta.DetalhesErros.FirstOrDefault()!.Campo.Should().Be("ValorNota");
    }
}