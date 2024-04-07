using System;
using ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLacamentoNotas.Testes.Comum;
using ServicoLancamentoNotas.Dominio.Params;
using Xunit;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Dominio.Enums;
using ServicoLancamentoNotas.Dominio.SeedWork.BuscaRepositorio;
using System.Linq;
using System.Collections.Generic;

namespace ServicoLacamentoNotas.Testes.Aplicacao.CasosDeUsosNota.Consultar
{
    [CollectionDefinition(nameof(ConsultaNotaTestsFixture))]
    public class ConsultarNotaTestsFixtureCollection : ICollectionFixture<ConsultaNotaTestsFixture> {}

    public class ConsultaNotaTestsFixture : BaseFixture
    {            
        public ListaNotaInput RetornaListBuscaInput()
            => new(1, 10, null, null, "");    

        public BuscaOuput<Nota> RetornaOutputRepositorio()
            => new(1, 10, 90, Enumerable.Range(0,10).Select(_ => RetornaNota()).ToList().AsReadOnly()); 
        
        public BuscaOuput<Nota> RetornaOutputRepositorioVazio()
            => new(1, 10, 0, new List<Nota>(0).AsReadOnly()); 
        
        public NotaParams RetornaValoresParametrosNotaValidos()
            => new(RetornaNumeroIdRandomico(), RetornaNumeroIdRandomico(), RetornaValorNotaAleatorioValido(), DateTime.Now);

        public Nota RetornaNota()
            => new(RetornaValoresParametrosNotaValidos());
    }
}