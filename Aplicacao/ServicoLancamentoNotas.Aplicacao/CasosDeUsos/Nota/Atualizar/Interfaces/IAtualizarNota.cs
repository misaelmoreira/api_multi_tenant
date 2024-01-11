using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces
{
    public interface IAtualizarNota : IRequestHandler<AtualizarNotaInput, NotaOutputModel>
    {
        
    }
}