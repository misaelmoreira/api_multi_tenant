using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;
using ServicoLancamentoNotas.Aplicacao.Comum;
using Microsoft.Extensions.Logging;
using ServicoLancamentoNotas.Aplicacao.Enums;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar
{
    public sealed class ConsultaNota : IConsultaNota
    {
        private readonly INotaRepository _notaRepository;
        private readonly ILogger<ConsultaNota> _logger;

        public ConsultaNota(INotaRepository notaRepository, ILogger<ConsultaNota> logger)
        {
            _notaRepository = notaRepository;
            _logger = logger;
        }

        public async Task<Resultado<ListaNotaOutput>> Handle(ListaNotaInput request, CancellationToken cancellationToken)
        {
            try
            {
                var buscaOutput = await _notaRepository.Buscar(new(request.Pagina, request.PorPagina, request.AlunoId, request.AtividadeId, request.OrdenarPor, request.Ordenacao), cancellationToken);
    
                ListaNotaOutput retorno = new ListaNotaOutput(buscaOutput.Pagina, buscaOutput.PorPagina, buscaOutput.Total, 
                    buscaOutput.Items.Select(nota => MapeadorAplicacao.NotaEmNotaOutpuModel(nota)).ToList().AsReadOnly());
    
                return Resultado<ListaNotaOutput>.RetornarResultadoSucesso(retorno);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Resultado<ListaNotaOutput>.RetornaResultadoErro(TipoErro.ErroInesperado);
            }
        }
    }
}