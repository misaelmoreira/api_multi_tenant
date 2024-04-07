using Bogus;
using Microsoft.EntityFrameworkCore;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLancamentoNotas.Dominio.Params;
using ServicoLancamentoNotas.Infra.Data.Contexto;

namespace ServicoLancamentoNotas.TestesIntegracao.Base;

public class BaseFixture 
{
    protected Faker Faker { get; } = new("pt_BR");

    public static int RetornaNumeroIdRandomico()
            => new Random().Next(1, 1_000_000);

    public double RetornaValorNotaAleatorioValido()
        => Faker.Random.Double(0.00, 10.00);

    public bool RetornaBoleanRandomico()
        => new Random().Next(0, 10) > 5; 

    public NotaParams RetornaValoresParametrosNotaValidos(int? idAluno = null)
        => new(idAluno ?? RetornaNumeroIdRandomico(), 
                RetornaNumeroIdRandomico(), 
                RetornaValorNotaAleatorioValido(), 
                DateTime.Now
            );

    public Nota RetornaNota(int? idAluno = null )
        => new(RetornaValoresParametrosNotaValidos(idAluno));
}