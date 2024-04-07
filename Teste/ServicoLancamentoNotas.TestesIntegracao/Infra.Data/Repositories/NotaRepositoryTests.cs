using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.Infra.Data.Repositories;
using Xunit;

namespace ServicoLancamentoNotas.TestesIntegracao.Infra.Data.Repositories;

[Collection(nameof(NotaRepositoryTestsFixture))]
public class NotaRepositoryTests
{
    private readonly INotaRepository _sut;
    private readonly NotaRepositoryTestsFixture _fixture;
    private ServicoLancamentoNotaDbContext _context;

    public NotaRepositoryTests(NotaRepositoryTestsFixture fixture)
    {
        _fixture = fixture;
        _context = _fixture.CriarDbContext();
        _sut = new NotaRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact(DisplayName = nameof(Inserir_QuandoFornecisaNota_DeveSalvar))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Inserir_QuandoFornecisaNota_DeveSalvar()
    {
        //arrange
        var nota = _fixture.RetornaNota();

        //act
        await _sut.Inserir(nota, CancellationToken.None);
        await _context.SaveChangesAsync();

        var notaSalva = _context.Notas.FirstOrDefault(x => x.Id == nota.Id);       

        //assert
        notaSalva.Should().NotBeNull();
        notaSalva!.AlunoId.Should().Be(nota.AlunoId);
        notaSalva.AtividadeId.Should().Be(nota.AtividadeId);
        notaSalva.ValorNota.Should().Be(nota.ValorNota);
        notaSalva.DataLancamento.Should().Be(nota.DataLancamento);
        notaSalva.DataCriacao.Should().BeCloseTo(nota.DataCriacao, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(BuscaNotaPorAlunoEAtividade_QuandoNotaExiste_DeveRetornar))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task BuscaNotaPorAlunoEAtividade_QuandoNotaExiste_DeveRetornar()
    {
        //arrange
        var nota = _fixture.RetornaNota();
        await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();

        //act
        var notaSalva = await _sut.BuscarNotaPorAlunoEAtividade(nota.AlunoId, nota.AtividadeId, CancellationToken.None);       

        //assert
        notaSalva.Should().NotBeNull();
        notaSalva!.AlunoId.Should().Be(nota.AlunoId);
        notaSalva.AtividadeId.Should().Be(nota.AtividadeId);
        notaSalva.ValorNota.Should().Be(nota.ValorNota);
        notaSalva.DataLancamento.Should().Be(nota.DataLancamento);
        notaSalva.DataCriacao.Should().BeCloseTo(nota.DataCriacao, TimeSpan.FromSeconds(1));
    }

    [Fact(DisplayName = nameof(Atualizar_QuandoNotaExiste_DeveAtualizarValores))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Atualizar_QuandoNotaExiste_DeveAtualizarValores()
    {
        // arrange
        var novoValorNota = 10;
        var nota = _fixture.RetornaNota();
        var tracking = await _context.Notas.AddAsync(nota);
        await _context.SaveChangesAsync();
        tracking.State = EntityState.Detached;
        nota!.AtualizarValorNota(novoValorNota); 

        // act
        await _sut.Atualizar(nota!, CancellationToken.None);  
        await _context.SaveChangesAsync();

        var notaSalva = await _context.Notas.FirstOrDefaultAsync(x => x.Id == nota.Id);      

        // assert
        notaSalva.Should().NotBeNull();
        notaSalva!.AlunoId.Should().Be(nota.AlunoId);
        notaSalva.AtividadeId.Should().Be(nota.AtividadeId);
        notaSalva.ValorNota.Should().Be(novoValorNota);
        notaSalva.DataLancamento.Should().Be(nota.DataLancamento);
        notaSalva.DataCriacao.Should().BeCloseTo(nota.DataCriacao, TimeSpan.FromSeconds(1));
    }


    [Fact(DisplayName = nameof(Buscar_QuandoNotaNaoExistemNotasCadastradas_DeveRetornarListaVazia))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoNotaNaoExistemNotasCadastradas_DeveRetornarListaVazia()
    {
        //arrange
        var input = _fixture.RetornarBuscaInputApenasComPaginacao();

        //act
        var resultado = await _sut.Buscar(input, CancellationToken.None);          

        //assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().BeEmpty();
        resultado.Total.Should().Be(default);
    }

    [Fact(DisplayName = nameof(Buscar_QuandoNotaExistemNotasCadastradas_DeveRetornarLista))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoNotaExistemNotasCadastradas_DeveRetornarLista()
    {
        //arrange
        var input = _fixture.RetornarBuscaInputApenasComPaginacao();
        var notas = _fixture.RetornarNotas();
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //act
        var resultado = await _sut.Buscar(input, CancellationToken.None);          

        //assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Total.Should().Be(notas.Count());
        foreach (var nota in resultado.Items)
        {
            var notaLista = notas.FirstOrDefault(x => x.Id == nota.Id);
            notaLista.Should().NotBeNull();
            nota.ValorNota.Should().Be(notaLista!.ValorNota);
            nota.AlunoId.Should().Be(notaLista.AlunoId);
            nota.AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }

    [Theory(DisplayName = nameof(Buscar_QuandoInformadoOrdenacao_DeveRetornarLista))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    [InlineData(OrdenacaoBusca.Desc, "alunoid")]
    [InlineData(OrdenacaoBusca.Asc, "alunoid")]
    [InlineData(OrdenacaoBusca.Desc, "atividadeid")]
    [InlineData(OrdenacaoBusca.Asc, "atividadeid")]
    public async Task Buscar_QuandoInformadoOrdenacao_DeveRetornarLista(OrdenacaoBusca ordenacao, string ordenarPor)
    {
        //arrange
        var input = _fixture.RetornarBuscaInputApenasComPaginacao(ordenacao: ordenacao, ordenarPor: ordenarPor);
        var notas = _fixture.RetornarNotas();
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //act
        var resultado = await _sut.Buscar(input, CancellationToken.None);          

        //assert
        var listaOrdenada = _fixture.NotasOrdenadas(notas, ordenarPor, ordenacao);
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Total.Should().Be(notas.Count());

        for (var index = 0; index < notas.Count(); index++)
        {
            var notaLista = listaOrdenada[index];
            notaLista.Should().NotBeNull();
            resultado.Items[index].ValorNota.Should().Be(notaLista!.ValorNota);
            resultado.Items[index].AlunoId.Should().Be(notaLista.AlunoId);
            resultado.Items[index].AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }

    [Fact(DisplayName = nameof(Buscar_QuandoInformadoAlunoId_DeveRetornarLista))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoInformadoAlunoId_DeveRetornarLista()
    {
        //arrange
        var notas = _fixture.RetornarNotas();
        var alunoId = notas[0].AlunoId;
        var input = _fixture.RetornarBuscaInputApenasComPaginacao(alunoId);
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //act
        var resultado = await _sut.Buscar(input, CancellationToken.None);          

        //assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Total.Should().Be(notas.Count(x => x.AlunoId == alunoId));
        foreach (var nota in resultado.Items)
        {
            var notaLista = notas.FirstOrDefault(x => x.Id == nota.Id);
            notaLista.Should().NotBeNull();
            nota.ValorNota.Should().Be(notaLista!.ValorNota);
            nota.AlunoId.Should().Be(notaLista.AlunoId);
            nota.AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }

    [Fact(DisplayName = nameof(Buscar_QuandoInformadoAtividadeId_DeveRetornarLista))]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoInformadoAtividadeId_DeveRetornarLista()
    {
        //arrange
        var notas = _fixture.RetornarNotas();
        var atividadeId = notas[0].AtividadeId;
        var input = _fixture.RetornarBuscaInputApenasComPaginacao(atividadeId: atividadeId);
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //act
        var resultado = await _sut.Buscar(input, CancellationToken.None);          

        //assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Total.Should().Be(notas.Count(x => x.AtividadeId == atividadeId));
        foreach (var nota in resultado.Items)
        {
            var notaLista = notas.FirstOrDefault(x => x.Id == nota.Id);
            notaLista.Should().NotBeNull();
            nota.ValorNota.Should().Be(notaLista!.ValorNota);
            nota.AlunoId.Should().Be(notaLista.AlunoId);
            nota.AtividadeId.Should().Be(notaLista.AtividadeId);
        }
    }

    [Theory(DisplayName = nameof(Buscar_QuandoInformadoPaginacao_DeveRetornarLista))]
    [InlineData(20, 1, 10, 10)]
    [InlineData(15, 2, 10, 5)]
    [Trait("Infra.Data", "Integracao/Repositories - Nota Repository")]
    public async Task Buscar_QuandoInformadoPaginacao_DeveRetornarLista(int quantidadeGerada, int pagina, int porPagina, int quantidadeItensEsperada)
    {
        //arrange
        var notas = _fixture.RetornarNotas(quantidadeGerada);
        var input = _fixture.RetornarBuscaInputApenasComPaginacao(pagina: pagina, porPagina: porPagina);
        await _context.AddRangeAsync(notas);
        await _context.SaveChangesAsync();

        //act
        var resultado = await _sut.Buscar(input, CancellationToken.None);          

        //assert
        resultado.Should().NotBeNull();
        resultado.Pagina.Should().Be(input.Pagina);
        resultado.PorPagina.Should().Be(input.PorPagina);
        resultado.Items.Should().NotBeEmpty();
        resultado.Items.Count().Should().Be(quantidadeItensEsperada);
        resultado.Total.Should().Be(notas.Count());
    }

}