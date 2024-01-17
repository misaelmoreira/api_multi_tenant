using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;
using ServicoLancamentoNotas.Dominio.Repositories;

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

        public async Task<NotaOutputModel> Handle(AtualizarNotaInput request, CancellationToken cancellationToken)
        {
            var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);

            nota.AtualizarValorNota(request.ValorNota);
            await _notaRepository.Atualizar(nota, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return MapeadorAplicacao.NotaEmNotaOutpuModel(nota);
        }

    }
}