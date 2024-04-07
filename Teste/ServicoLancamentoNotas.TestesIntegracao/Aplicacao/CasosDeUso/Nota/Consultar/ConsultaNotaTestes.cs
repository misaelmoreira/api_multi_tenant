using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Dominio.Constantes;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.Infra.Data.Repositories;
using ServicoLancamentoNotas.Infra.Data.UoW;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Consulta;

[Collection(nameof(ConsultaNotasTestsFixture))]
public class ConsultaNotaTests 
{
    private readonly ConsultaNotasTestsFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotaRepository _notaRepository;
    private readonly ILogger<ConsultaNota> _logger;
    private readonly IConsultaNota _sut;
    private readonly ServicoLancamentoNotaDbContext _context;

    public ConsultaNotaTests(ConsultaNotasTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _unitOfWork = new UnitOfWork(_context);
        _notaRepository = new NotaRepository(_context);
        var loggerFactory = new LoggerFactory();
        _logger = loggerFactory.CreateLogger<ConsultaNota>();
        _sut = new ConsultaNota(_notaRepository, _logger);        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaRetornaValores_DeveRetornarResultadoComSucessoEValores))]
    [Trait("Aplicacao", "Integracao/ConsultaNota - Casos de Uso")]
    public async Task Handle_QuandoBuscaRetornaValores_DeveRetornarResultadoComSucessoEValores()
    {
        // arrange
        var notas = _fixture.RetornaNotasValidas();
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();
        var buscaInput = _fixture.RetornaListBuscaInput();
        
        //act
        var output = await _sut.Handle(buscaInput, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
        output.Sucesso.Should().BeTrue();
        output.Erro.Should().BeNull();
        output.DescricaoErro.Should().BeNull();
        output.Dado.Total.Should().Be(notas.Count);
        output.Dado.Pagina.Should().Be(buscaInput.Pagina);
        output.Dado.PorPagina.Should().Be(buscaInput.PorPagina);
        output.Dado.Items.Should().HaveCount(buscaInput.PorPagina);
        output.Dado.Items.ToList().ForEach(item =>
        {
            var nota = notas.FirstOrDefault(x => x.AtividadeId == item.AtividadeId && x.AlunoId == item.AlunoId);

            nota.Should().NotBeNull();
            item.ValorNota.Should().Be(nota!.ValorNota);
            item.StatusIntegracao.Should().Be(nota.StatusIntegracao);
            item.Cancelada.Should().Be(nota.Cancelada);
        });   
    }

    [Fact(DisplayName = nameof(Handle_QuandoBuscaNaoRetornaValores_DeveRetornarResultadoComSucessoOutputVazio))]
    [Trait("Aplicacao", "Integracao/ConsultaNota - Casos de Uso")]
    public async Task Handle_QuandoBuscaNaoRetornaValores_DeveRetornarResultadoComSucessoOutputVazio()
    {
        // arrange
        var input = _fixture.RetornaListBuscaInput();

        //act
        var output = await _sut.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Should().BeAssignableTo<Resultado<ListaNotaOutput>>();
        output.Sucesso.Should().BeTrue();            
        output.Erro.Should().BeNull();
        output.DescricaoErro.Should().BeNull();
        output.Dado.Total.Should().Be(default);
        output.Dado.Pagina.Should().Be(input.Pagina);
        output.Dado.PorPagina.Should().Be(input.PorPagina);
        output.Dado.Items.Should().HaveCount(default(int)); 
    }

    [Theory(DisplayName = nameof(Handle_QuandoBuscar_DeveRetornarResultadoComSucessoEValores))]
    [Trait("Aplicacao", "Integracao/ConsultaNota - Casos de Uso")]
    [InlineData(OrdenacaoBusca.Desc, "alunoid")]
    [InlineData(OrdenacaoBusca.Asc, "alunoid")]
    [InlineData(OrdenacaoBusca.Desc, "atividadeid")]
    [InlineData(OrdenacaoBusca.Asc, "atividadeid")]
    public async Task Handle_QuandoBuscar_DeveRetornarResultadoComSucessoEValores(OrdenacaoBusca ordenacao, string ordenarPor)
    {
        // arrange        
        var notas = _fixture.RetornaNotasValidas();
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();
        var buscaInput = _fixture.RetornarBuscaInputApenasComPaginacao(porPagina: 20,ordenacao: ordenacao, ordenarPor: ordenarPor);

        //act
        var output = await _sut.Handle(buscaInput, CancellationToken.None);

        //Assert        
        output.Should().NotBeNull();
        output.Dado.Pagina.Should().Be(buscaInput.Pagina);
        output.Dado.PorPagina.Should().Be(buscaInput.PorPagina);
        output.Dado.Items.Should().NotBeEmpty();
        output.Dado.Total.Should().Be(notas.Count());

        var listaOrdenada = _fixture.NotasOrdenadas(notas, ordenarPor, ordenacao);
        for (var index = 0; index < notas.Count(); index++)
        {
            var notaLista = listaOrdenada[index];
            notaLista.Should().NotBeNull();
            output.Dado.Items[index].ValorNota.Should().Be(notaLista!.ValorNota);
            output.Dado.Items[index].AlunoId.Should().Be(notaLista.AlunoId);
            output.Dado.Items[index].AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }    
}