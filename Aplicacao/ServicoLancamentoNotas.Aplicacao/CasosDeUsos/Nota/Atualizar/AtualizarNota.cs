using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Dominio.SeedWork;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar
{
    public sealed class AtualizarNota : IAtualizarNota
    {
        private readonly INotaRepository _notaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AtualizarNota(INotaRepository notaRepository, IUnitOfWork unitOfWork)
        {
            _notaRepository = notaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<NotaOutputModel> Handle(LancarNotaInput request, CancellationToken cancellationToken)
        {
            var nota = MapeadorAplicacao.LancarNotaInputEmNota(request);

            await _notaRepository.Inserir(nota, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return MapeadorAplicacao.NotaEmNotaOutpuModel(nota);
        }
    }
}