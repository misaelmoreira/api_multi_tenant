using System;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLacamentoNotas.Testes.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using ServicoLancamentoNotas.Dominio.Params;
using Xunit;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Cancelar
{
    [CollectionDefinition(nameof(CancelarNotaTestsFixture))]
    public class CancelarNotaTestsFixtureCollection : ICollectionFixture<CancelarNotaTestsFixture> {}

    public class CancelarNotaTestsFixture : BaseFixture
    {            
        public CancelarNotaInput RetornaInputValido()
            => new(RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    Faker.Commerce.ProductName());    

        public CancelarNotaInput RetornaInputInvalido()
        {
            string motivoCancelamento = Faker.Lorem.Text();
            while(motivoCancelamento.Length <= 500)
                motivoCancelamento += Faker.Lorem.Text();

            return new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(),  RetornaNumeroIdRandomico(), motivoCancelamento);
        } 

        public NotaParams RetornaValoresParametrosNotaValidos()
            => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), DateTime.Now);

        public Nota RetornaNota()
            => new(RetornaValoresParametrosNotaValidos());
    }
}