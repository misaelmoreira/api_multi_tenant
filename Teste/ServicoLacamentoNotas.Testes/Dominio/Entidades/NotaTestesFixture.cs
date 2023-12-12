using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicoLacamentoNotas.Testes.Comum;
using ServicoLancamentoNotas.Dominio.Params;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Dominio.Entidades
{
    public class NotaTestesFixture : BaseFixture
    {
        public static int RetornaNumeroIdRandomico()
            => new Random().Next(1, 1_000_000);

        public double RetornaValorNotaAleatorioValido()
            => Faker.Random.Double(0.00, 10.00);

        public NotaParams RetornaValoresParametrosInvalidosCustomizados(int? alunoId = null, int? atividadeId =null, double? valorNota =null, int? usuarioId = null)
            => new(alunoId ?? RetornaNumeroIdRandomico(), atividadeId ?? RetornaNumeroIdRandomico(), valorNota ?? RetornaValorNotaAleatorioValido(), DateTime.Now, usuarioId ?? RetornaNumeroIdRandomico());


    
        public NotaParams RetornaValoresParametrosNotaValidos()
            => new (RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), DateTime.Now, RetornaNumeroIdRandomico());
                
    }
    
    [CollectionDefinition(nameof(NotaTestesFixture))]
    public class NotaTestesFixtureCollection : ICollectionFixture<NotaTestesFixture> {}
}