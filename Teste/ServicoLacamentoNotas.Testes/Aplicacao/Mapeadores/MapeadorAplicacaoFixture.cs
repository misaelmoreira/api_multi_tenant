using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLacamentoNotas.Testes.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Dominio.Params;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Aplicacao.Mapeadores
{
    public class MapeadorAplicacaoFixtureCollection : ICollectionFixture<MapeadorAplicacaoFixture> {}

    public class MapeadorAplicacaoFixture : BaseFixture
    {
        public static int RetornaNumeroIdRandomico()
            => new Random().Next(1, 1_000_000);

        public double RetornaValorNotaAleatorioValido()
            => Faker.Random.Double(0.00, 10.00);

        public bool RetornaBoleanRandomico()
            => new Random().Next(0, 10) > 5;

        public LancarNotaInput DevolveNotaInputValido()
            => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), RetornaBoleanRandomico());  

        public NotaParams RetornaValoresParametrosNotaValidos()
            => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), DateTime.Now); 


        public Nota RetornaNotaValida()  
            => new(RetornaValoresParametrosNotaValidos());   
    }
}