using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.Interfaces;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Dominio.Repositories;
using ServicoLancamentoNotas.Aplicacao.Mapeadores;

namespace ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar
{
    public sealed class ConsultarNota : IConsultaNota
    {
        private readonly INotaRepository _notaRepository;

        public ConsultarNota(INotaRepository notaRepository)
        {
            _notaRepository = notaRepository;
        }

        public async Task<ListaNotaOutput> Handle(ListaNotaInput request, CancellationToken cancellationToken)
        {
            var buscaOutput = await _notaRepository.Buscar(new(request.Pagina, request.PorPagina, request.AlunoId, request.AtividadeId, request.OrdenarPor, request.OrdenacaoBusca), cancellationToken);

            return new(buscaOutput.Pagina, buscaOutput.PorPagina, buscaOutput.Total, 
                buscaOutput.Items.Select(nota => MapeadorAplicacao.NotaEmNotaOutpuModel(nota)).ToList().AsReadOnly());
        }
    }
}