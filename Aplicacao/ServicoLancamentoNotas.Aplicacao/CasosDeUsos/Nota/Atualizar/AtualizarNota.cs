using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Interfaces;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.Enums;
using Microsoft.Extensions.Logging;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar
{
    public sealed class AtualizarNota : IAtualizarNota
    {
        private readonly INotaRepository _notaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AtualizarNota> _logger;

        public AtualizarNota(INotaRepository notaRepository, 
                            IUnitOfWork unitOfWork, 
                            ILogger<AtualizarNota> logger)
        {
            _notaRepository = notaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Resultado<NotaOutputModel>> Handle(AtualizarNotaInput request, CancellationToken cancellationToken)
        {
            try 
            {
                var nota = await _notaRepository.BuscarNotaPorAlunoEAtividade(request.AlunoId, request.AtividadeId, cancellationToken);
                if(nota is null)
                    return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaNaoEncontrada);

                nota.AtualizarValorNota(request.ValorNota);

                if(!nota.EhValida)
                    return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.NotaInvalida, nota.Notificacoes.Select(notificacao => new DetalheErro(notificacao.Campo, notificacao.Mensagem)).ToList());

                await _notaRepository.Atualizar(nota, cancellationToken);            
                await _unitOfWork.Commit(cancellationToken);

                return Resultado<NotaOutputModel>.RetornarResultadoSucesso(MapeadorAplicacao.NotaEmNotaOutpuModel(nota));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Resultado<NotaOutputModel>.RetornaResultadoErro(TipoErro.ErroInesperado);
            }
        }
    }
}