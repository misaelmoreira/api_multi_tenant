using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicoLancamentoNotas.Dominio.SeedWork
{
    public abstract class NotifiableObject
    {
        public bool EhValida { get; set; }
        public List<Notificacao> Notificacoes { get; } = new();

        public void Notificar(Notificacao notificacao)
            => Notificacoes.Add(notificacao);
    }
}