using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicoLancamentoNotas.Dominio.Specifications;

namespace Dominio.ServicoLancamentoNotas.Dominio.Entidades;

public partial class Nota
{
    private bool PodeAlterarStatusParaEnviado()
        => StatusAguardandoIntegracaoSpec.Instance
            .IsSatisfied(this);   

    private bool PodeAlterarStatusParaFalhaIntegracao()
        => StatusEnviadaParaIntegracaoSpec.Instance
            .IsSatisfied(this);   

    private bool PodeAlterarStatusParaIntegradaComSucesso()
        => StatusEnviadaParaIntegracaoSpec.Instance
            .IsSatisfied(this);   
}