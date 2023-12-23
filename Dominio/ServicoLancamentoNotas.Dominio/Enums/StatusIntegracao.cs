using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ServicoLancamentoNotas.Dominio.Enums;
public enum StatusIntegracao
{
    
    AguardandoIntegracao = 1,
    EviadaParaIntegracao = 2,
    IntegradaComSucesso = 3,
    FalhaNaIntegracao = 4
}