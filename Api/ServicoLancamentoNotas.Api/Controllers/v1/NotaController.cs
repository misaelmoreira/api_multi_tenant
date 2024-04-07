using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Atualizar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Cancelar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Comum;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Consultar.DTOs;
using ServicoLancamentoNotas.Aplicacao.CasosDeUsos.Nota.Lancar.DTOs;
using ServicoLancamentoNotas.Aplicacao.Comum;
using ServicoLancamentoNotas.Aplicacao.Interfaces;

namespace ServicoLancamentoNotas.Api.Controllers.v1;

[ApiController]
[ApiVersion("1")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("notas/v1")]
public class NotaController : ControllerBase
{
    private readonly ILogger<NotaController> _logger;
    private readonly IMediatorHandler _handler;
    public NotaController(ILogger<NotaController> logger, IMediatorHandler handler)
    {
        _logger = logger;
        _handler = handler;
    }

    [HttpPost("lancar")]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Lancar([FromBody] LancarNotaInput input, CancellationToken cancellationToken)
    {
        var response = await _handler.EnviarRequest<Resultado<NotaOutputModel>, LancarNotaInput>(input, cancellationToken);

        _logger.LogInformation("Efetuado lancamento de nota para o aluno {alunoId} para a atividade {atividadeId}. {@response}", input.AlunoId, input.AtividadeId, response);

        if (response.Sucesso)
            return Ok(response);
        
        return BadRequest(response);
    }

    [HttpPatch("atualizar")]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Atualizar([FromBody] AtualizarNotaInput input, CancellationToken cancellationToken)
    {
        var response = await _handler.EnviarRequest<Resultado<NotaOutputModel>, AtualizarNotaInput>(input, cancellationToken);

        _logger.LogInformation("Efetuado atualização de nota para o aluno {alunoId} para a atividade {atividadeId}. {@response}", input.AlunoId, input.AtividadeId, response);

        if (response.Sucesso)
            return Ok(response);
        
        return BadRequest(response);
    }

    [HttpPatch("cancelar")]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancelar([FromBody] CancelarNotaInput input, CancellationToken cancellationToken)
    {
        var response = await _handler.EnviarRequest<Resultado<NotaOutputModel>, CancelarNotaInput>(input, cancellationToken);

        _logger.LogInformation("Efetuado cancelamento de nota para o aluno {alunoId} para a atividade {atividadeId}. {@response}", input.AlunoId, input.AtividadeId, response);

        if (response.Sucesso)
            return Ok(response);
        
        return BadRequest(response);
    }

    [HttpPost("buscar")]
    [ProducesResponseType(typeof(Resultado<NotaOutputModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] ListaNotaInput input, CancellationToken cancellationToken)
    {
        var response = await _handler.EnviarRequest<Resultado<ListaNotaOutput>, ListaNotaInput>(input, cancellationToken);

        return Ok(response);        
    }
}