using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.ServicoLancamentoNotas.Dominio.Entidades;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Dominio.Params;

namespace ServicoLancamentoNotas.Aplicacao.Mapeadores
{
    public class MapeadorAplicacao
    {
        public static Nota LancarNotaInputEmNota(LancarNotaInput input)
        {
            var notaParams = new NotaParams(input.AlunoId, input.AtividadeId, input.ValorNota, DateTime.Now);

            return new(notaParams);
        }

        public static NotaOutputModel  NotaEmNotaOutpuModel(Nota nota) 
            => new(nota.AlunoId, nota.AtividadeId, nota.ValorNota, nota.DataLancamento, nota.Cancelada, nota.MotivoCancelamento, nota.StatusIntegracao);
    }
}