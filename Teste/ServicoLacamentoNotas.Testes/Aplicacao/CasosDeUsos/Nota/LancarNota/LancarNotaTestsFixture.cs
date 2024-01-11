using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicoLacamentoNotas.Testes.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsos.Nota.LancarNota
{
    [CollectionDefinition(nameof(LancarNotaTestsFixture))]
    public class LancarNotaTestsFixtureCollection : ICollectionFixture<LancarNotaTestsFixture> {}

    public class LancarNotaTestsFixture : BaseFixture
    {
        public static int RetornaNumeroIdRandomico()
            => new Random().Next(1, 1_000_000);

        public double RetornaValorNotaAleatorioValido()
            => Faker.Random.Double(0.00, 10.00);

        public bool RetornaBoleanRandomico()
            => new Random().Next(0, 10) > 5;
            
        public LancarNotaInput DevolveNotaInputValido()
            => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), RetornaBoleanRandomico());        
    }
}