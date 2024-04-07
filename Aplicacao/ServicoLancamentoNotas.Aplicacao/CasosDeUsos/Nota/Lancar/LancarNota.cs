using Microsoft.Extensions.Logging;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.Enums;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;
using ServicoLancamentoNotas.Dominio.Repositories;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar
{
    public sealed class LancarNota : ILancarNota
    {
        private readonly INotaRepository _notaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LancarNota> _logger;
        public LancarNota(INotaRepository notaRepository, 
                            IUnitOfWork unitOfWork,
                            ILogger<LancarNota> logger)
        {
            _notaRepository = notaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Resultado<NotaOutputModel>> Handle(LancarNotaInput request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.NotaSubstitutiva)
                    await TentarCancelarNota(request, cancellationToken);
    
                var novaNota = MapeadorAplicacao.LancarNotaInputEmNota(request);
    
                if(!novaNota.EhValida)
                        return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaInvalida, novaNota.Notificacoes.Select(notificacao => new DetalheErro(notificacao.Campo, notificacao.Mensagem)).ToList());
    
                await _notaRepository.Inserir(novaNota, cancellationToken);
                await _unitOfWork.Commit(cancellationToken);
    
                return Resultado<NotaOutputModel>.RetornarResultadoSucesso(MapeadorAplicacao.NotaEmNotaOutpuModel(novaNota));
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex, ex.Message);
                return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.ErroInesperado);
            }
        }
    
        private async Task TentarCancelarNota(LancarNotaInput request, CancellationToken cancellationToken)
        {
            var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);
            if(nota is not null)
            {
                nota.CancelarPorRetentativa();
                await _notaRepository.Atualizar(nota, cancellationToken);
            }
        }
    }
}