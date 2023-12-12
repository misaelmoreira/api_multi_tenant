using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicoLancamentoNotas.Dominio.SeedWork
{
    public class Notificacao
    {
        public string Campo { get; }
        public string Mensagem { get; }
        
        public Notificacao(string campo, string mensagem)
        {
            Campo = campo;
            Mensagem = mensagem;
        }        
    }
}