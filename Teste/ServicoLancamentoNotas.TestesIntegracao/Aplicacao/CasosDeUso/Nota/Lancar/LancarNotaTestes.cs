using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.Enums;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Dominio.Constantes;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.Infra.Data.Repositories;
using ServicoLancamentoNotas.Infra.Data.UoW;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Lancar;

[Collection(nameof(LancarNotasTestsFixture))]
public class LancarNotaTests 
{
    private readonly LancarNotasTestsFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<LancarNota> _logger;
    private readonly ILancarNota _sut;
    private readonly ServicoLancamentoNotaDbContext _context;

    public LancarNotaTests(LancarNotasTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _unitOfWork = new UnitOfWork(_context);
        _notaRepository = new NotaRepository(_context);
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<LancarNota>();
        _sut = new LancarNota(_notaRepository, _unitOfWork, _logger);        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaValida_DeveSerSalva))]
    [Trait("Aplicacao", "Integracao/LancarNota - Casos de Uso")]
    public async Task Handle_QuandoNotaValida_DeveSerSalva()
    {
        // arrange
        var input = _fixture.DevolveNotaInputValido();
        
        //act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Dado.Should().NotBeNull();
        output.Dado.ValorNota.Should().Be(input.ValorNota);
        output.Dado.AtividadeId.Should().Be(input.AtividadeId);
        output.Dado.AlunoId.Should().Be(input.AlunoId);  
        output.Dado.Cancelada.Should().BeFalse();          

        var notaSalva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == input.AlunoId && x.AtividadeId == input.AtividadeId);

        notaSalva.Should().NotBeNull();
        notaSalva!.ValorNota.Should().Be(input.ValorNota);
        notaSalva.Cancelada.Should().BeFalse();
        notaSalva.StatusIntegracao.Should().Be(StatusIntegracao.AguardandoIntegracao);
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaInvalida_NaoDeveSerSalva))]
    [Trait("Aplicacao", "Integracao/LancarNota - Casos de Uso")]
    public async Task Handle_QuandoNotaInvalida_NaoDeveSerSalva()
    {
        // arrange
        var input = _fixture.DevolveNotaInputInvalido();

        //act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();
        output.Dado.Should().BeNull();
        output.Sucesso.Should().BeFalse();
        output.DetalhesErros.Should().NotBeEmpty();
        output.DetalhesErros.Should().HaveCount(3);
        output.Erro.Should().Be(TipoErro.NotaInvalida);

        var notaSalva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == input.AlunoId && x.AtividadeId == input.AtividadeId);
        notaSalva.Should().BeNull();
    }

    [Fact(DisplayName = nameof(Handle_QuandoNotaNotaValidaParaSerSalvaESusbistutiva_DeveSerSalvarEAtualizarNotaSubstituida))]
    [Trait("Aplicacao", "Integracao/LancarNota - Casos de Uso")]
    public async Task Handle_QuandoNotaNotaValidaParaSerSalvaESusbistutiva_DeveSerSalvarEAtualizarNotaSubstituida()
    {
        // arrange        
        var nota = _fixture.RetornaNota();
        var input = _fixture.DevolveNotaSubstitutivaInputValido(nota.AlunoId, nota.AtividadeId);
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;

        //act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeOfType<Resultado<NotaOutputModel>>();      
        output.Dado.Should().NotBeNull();
        output.Dado.ValorNota.Should().Be(input.ValorNota);
        output.Dado.AtividadeId.Should().Be(input.AtividadeId);
        output.Dado.AlunoId.Should().Be(input.AlunoId);
        output.Dado.Cancelada.Should().BeFalse();

        var notaCancelada = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == input.AlunoId && x.AtividadeId == input.AtividadeId && x.Cancelada);

        notaCancelada.Should().NotBeNull();
        notaCancelada!.Cancelada.Should().BeTrue();
        notaCancelada.CanceladaPorRetentativa.Should().BeTrue();
        notaCancelada.MotivoCancelamento.Should().Be(ConstantesDominio.Mensagens.NOTA_CANCELADA_POR_RETENTATIVA);

        var notaAtiva = await _context.Notas
            .FirstOrDefaultAsync(x => x.AlunoId == input.AlunoId && x.AtividadeId == input.AtividadeId && !x.Cancelada);

        notaAtiva.Should().NotBeNull();
    }
    
}