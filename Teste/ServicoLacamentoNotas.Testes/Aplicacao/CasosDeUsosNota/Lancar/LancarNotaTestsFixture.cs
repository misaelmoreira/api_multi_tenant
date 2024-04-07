using System;
using ServicoLacamentoNotas.Testes.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLancamentoNotas.Dominio.Params;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Lancar
{
    [CollectionDefinition(nameof(LancarNotaTestsFixture))]
    public class LancarNotaTestsFixtureCollection : ICollectionFixture<LancarNotaTestsFixture> {}

    public class LancarNotaTestsFixture : BaseFixture
    {            
        public LancarNotaInput DevolveNotaInputValido()
            => new(RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaValorNotaAleatorioValido(), 
                    false
                );     

        public LancarNotaInput DevolveNotaInputValidoSubstitutivo()
            => new(RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaValorNotaAleatorioValido(), 
                    true
                ); 

        public LancarNotaInput DevolveNotaInputInvalido()
            => new(-1, -1, -1, 11, false);      

        public NotaParams RetornaValoresParametrosNotaValidos()
            => new(RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaValorNotaAleatorioValido(), 
                    DateTime.Now
                );

        public Nota RetornaNota()
            => new(RetornaValoresParametrosNotaValidos());
      
    }
}