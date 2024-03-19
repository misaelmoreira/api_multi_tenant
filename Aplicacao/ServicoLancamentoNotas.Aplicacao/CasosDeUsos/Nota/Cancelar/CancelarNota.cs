using Microsoft.Extensions.Logging;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.Enums;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;
using ServicoLancamentoNotas.Dominio.Repositories;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar
{
    public sealed class CancelarNota : ICancelarNota
    {
        private readonly INotaRepository _notaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CancelarNota> _logger;

        public CancelarNota(INotaRepository notaRepository, IUnitOfWork unitOfWork, ILogger<CancelarNota> logger)
        {
            _notaRepository = notaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Resultado<NotaOutputModel>> Handle(CancelarNotaInput request, CancellationToken cancellationToken)
        {
            try
            {
                var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);

                if(nota is null)
                    return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaNaoEncontrada);
                    
                nota.Cancelar(request.Motivo);

                if(!nota.EhValida)
                    return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaInvalida, nota.Notificacoes.Select(notificacao => new DetalheErro(notificacao.Campo, notificacao.Mensagem)).ToList());

                await _notaRepository.Atualizar(nota, cancellationToken);
                await _unitOfWork.Commit(cancellationToken);
    
                return Resultado<NotaOutputModel>.RetornarResultadoSucesso(MapeadorAplicacao.NotaEmNotaOutpuModel(nota));
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, ex.Message);
                return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.ErroInesperado);
            }
        }
    }
}