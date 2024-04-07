using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Infra.Data.Contexto;
using ServicoLancamentoNotas.TestesIntegracao.Base;
using Xunit;
using DominioEntidade = ServicoLancamentoNotas.Dominio.Entidades;

namespace ServicoLancamentoNotas.TestesIntegracao.Aplicacao.CasosDeUso.Nota.Consulta;

[CollectionDefinition(nameof(ConsultaNotasTestsFixture))]
public class ConsultaTestsFixtureCollection
    : ICollectionFixture<ConsultaNotasTestsFixture>
{ }

public class ConsultaNotasTestsFixture
    :  BaseFixture
{
    public ListaNotaInput RetornaListBuscaInput()
            => new(1, 10, null, null, "");  

    public List<DominioEntidade.Nota> RetornaNotasValidas()
            => Enumerable.Range(1, 20).Select(id => RetornaNota(id)).ToList(); 

    public List<DominioEntidade.Nota> NotasOrdenadas(IEnumerable<DominioEntidade.Nota> lista, string ordenarPor, OrdenacaoBusca ordenacao)
        => (ordenacao, ordenarPor.ToLower()) switch
        {
            (OrdenacaoBusca.Asc, "atividadeid") => lista.OrderBy(x => x.AtividadeId).ToList(),
            (OrdenacaoBusca.Desc, "atividadeid") => lista.OrderByDescending(x => x.AtividadeId).ToList(),
            (OrdenacaoBusca.Asc, "alunoid") => lista.OrderBy(x => x.AlunoId).ToList(),
            (OrdenacaoBusca.Desc, "alunoid") => lista.OrderByDescending(x => x.AlunoId).ToList(),
            _ => lista.OrderBy(x => x.AlunoId).ToList()
        };

    public ListaNotaInput RetornarBuscaInputApenasComPaginacao(int? alunoId = null, int? atividadeId = null, OrdenacaoBusca ordenacao = OrdenacaoBusca.Asc, string ordenarPor = "", int? pagina = null, int? porPagina = null)
        => new(pagina ?? 1, porPagina ?? 10, alunoId ?? null, atividadeId ?? null, ordenarPor, ordenacao); 

    // public ConsultaNotaInput DevolveNotaInputValido(int? alunoId = null,  int? atividadeId = null, double?valorNota = null)
    //     => new (alunoId ?? RetornaNumeroIdRandomico(), atividadeId ?? RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), valorNota ?? RetornaValorNotaAleatorioValido(), false);

    // public ConsultaNotaInput DevolveNotaSubstitutivaInputValido(int alunoId,  int atividadeId)
    //     => new (alunoId, atividadeId, RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), true);

    // public ConsultaNotaInput DevolveNotaInputInvalido()
    //         => new(-1, -1, -1, 11, false);  

    public ServicoLancamentoNotaDbContext CriarDbContext()
    {
        var dbContext = new ServicoLancamentoNotaDbContext(
            new DbContextOptionsBuilder<ServicoLancamentoNotaDbContext>()
                .UseInMemoryDatabase("integration-tests-consultar-nota")
                .Options
        );

        return dbContext;
    }
}