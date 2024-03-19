using System;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLacamentoNotas.Testes.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using ServicoLancamentoNotas.Dominio.Params;
using Xunit;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Atualizar
{
    [CollectionDefinition(nameof(AtualizarNotaTestsFixture))]
    public class AtualizarNotaTestsFixtureCollection : ICollectionFixture<AtualizarNotaTestsFixture> {}

    public class AtualizarNotaTestsFixture : BaseFixture
    {            
        public AtualizarNotaInput RetornaInputValido()
            => new(RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaValorNotaAleatorioValido());    
        
        public AtualizarNotaInput RetornaInputInvalido()
            => new(RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    RetornaNumeroIdRandomico(), 
                    -1);  

        public NotaParams RetornaValoresParametrosNotaValidos()
            => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), DateTime.Now);

        public Nota RetornaNota()
            => new(RetornaValoresParametrosNotaValidos());
    }
}