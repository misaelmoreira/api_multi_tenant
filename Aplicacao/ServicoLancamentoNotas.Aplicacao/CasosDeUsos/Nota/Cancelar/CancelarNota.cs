using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;
using ServicoLancamentoNotas.Dominio.Repositories;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar
{
    public sealed class CancelarNota : ICancelarNota
    {
        private readonly INotaRepository _notaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelarNota(INotaRepository notaRepository, IUnitOfWork unitOfWork)
        {
            _notaRepository = notaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<NotaOutputModel> Handle(CancelarNotaInput request, CancellationToken cancellationToken)
        {
            var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);
            nota.Cancelar(request.Motivo);
            await _notaRepository.Atualizar(nota, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return MapeadorAplicacao.NotaEmNotaOutpuModel(nota);
        }
    }
}