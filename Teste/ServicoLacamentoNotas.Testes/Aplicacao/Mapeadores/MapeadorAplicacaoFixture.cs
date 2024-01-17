using System;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLacamentoNotas.Testes.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Dominio.Params;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Aplicacao.Mapeadores
{
    [CollectionDefinition(nameof(MapeadorAplicacaoFixture))]
    public class MapeadorAplicacaoFixtureCollection : ICollectionFixture<MapeadorAplicacaoFixture> {}
    
    public class MapeadorAplicacaoFixture : BaseFixture
    {
        public LancarNotaInput DevolveNotaInputValido()
            => new(RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaValorNotaAleatorioValido(), 
                    RetornaBoleanRandomico()
                );  

        public NotaParams RetornaValoresParametrosNotaValidos()
            => new(RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaValorNotaAleatorioValido(), 
                    DateTime.Now
                ); 

        public Nota RetornaNotaValida()  
            => new(RetornaValoresParametrosNotaValidos());   
    }
}