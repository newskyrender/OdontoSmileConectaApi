using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Common;
using Integration.Domain.Enums;
using Integration.Service.Services;
using System.ComponentModel.DataAnnotations;

namespace Integration.Api.Controllers
{
    [Route("api-integration/solicitacao-orcamento")]
    [ApiController]
    public class SolicitacaoOrcamentoController : BaseController
    {
        private readonly SolicitacaoOrcamentoService _service;

        public SolicitacaoOrcamentoController(SolicitacaoOrcamentoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna a solicitação de orçamento filtrada pelo id
        /// </summary>
        /// <param name="id">ID da solicitação</param>
        /// <response code="200">Solicitação que foi retornada com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<SolicitacaoOrcamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            var data = await _service.Handle(id);
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista de todas as solicitações de orçamento
        /// </summary>
        /// <response code="200">Solicitações que foram retornadas com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<SolicitacaoOrcamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.Listar();
            return Ok(data);
        }

        /// <summary>
        /// Retorna as solicitações por status
        /// </summary>
        /// <param name="status">Status da solicitação (Pendente, EmAnalise, Aprovado, Recusado, Cancelado)</param>
        /// <response code="200">Solicitações que foram retornadas com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("status/{status}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<SolicitacaoOrcamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByStatus([Required] StatusSolicitacao status)
        {
            var data = await _service.GetPorStatus(status);
            return Ok(data);
        }

        /// <summary>
        /// Cadastra uma nova solicitação de orçamento
        /// </summary>
        /// <param name="request">Dados da solicitação para cadastro</param>
        /// <response code="200">Solicitação que foi inserida com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<SolicitacaoOrcamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Create([FromBody] SolicitacaoOrcamentoRegisterRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Aprova uma solicitação de orçamento
        /// </summary>
        /// <param name="request">Dados para aprovação do orçamento</param>
        /// <response code="200">Solicitação que foi aprovada com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost("aprovar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<SolicitacaoOrcamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Aprovar([FromBody] AprovarOrcamentoRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Altera o status de uma solicitação
        /// </summary>
        /// <param name="id">ID da solicitação</param>
        /// <param name="status">Novo status da solicitação</param>
        /// <response code="200">Status da solicitação alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPatch("{id:guid}/status")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<SolicitacaoOrcamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> AlterarStatus([Required] Guid id, [FromBody] StatusSolicitacao status)
        {
            var data = await _service.AlterarStatus(id, status);
            return Ok(data);
        }

        /// <summary>
        /// Remove uma solicitação (exclusão física)
        /// </summary>
        /// <param name="id">ID da solicitação</param>
        /// <response code="200">Solicitação que foi removida com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpDelete("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<SolicitacaoOrcamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Delete([Required] Guid id)
        {
            var data = await _service.Delete(id);
            return Ok(data);
        }

        /// <summary>
        /// Retorna as solicitações por paciente
        /// </summary>
        /// <param name="pacienteId">ID do paciente</param>
        /// <response code="200">Solicitações que foram retornadas com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("paciente/{pacienteId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<SolicitacaoOrcamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByPaciente([Required] Guid pacienteId)
        {
            var data = await _service.GetPorPaciente(pacienteId);
            return Ok(data);
        }

        /// <summary>
        /// Retorna as solicitações por profissional
        /// </summary>
        /// <param name="profissionalId">ID do profissional</param>
        /// <response code="200">Solicitações que foram retornadas com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("profissional/{profissionalId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<SolicitacaoOrcamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByProfissional([Required] Guid profissionalId)
        {
            var data = await _service.GetPorProfissional(profissionalId);
            return Ok(data);
        }

        /// <summary>
        /// Retorna as solicitações por tipo de tratamento
        /// </summary>
        /// <param name="tipoTratamento">Tipo de tratamento</param>
        /// <response code="200">Solicitações que foram retornadas com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("tipo-tratamento/{tipoTratamento}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<SolicitacaoOrcamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByTipoTratamento([Required] TipoTratamento tipoTratamento)
        {
            var data = await _service.GetPorTipoTratamento(tipoTratamento);
            return Ok(data);
        }

        /// <summary>
        /// Retorna as solicitações por período
        /// </summary>
        /// <param name="dataInicio">Data de início</param>
        /// <param name="dataFim">Data de fim</param>
        /// <response code="200">Solicitações que foram retornadas com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("periodo")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<SolicitacaoOrcamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByPeriodo([Required] DateTime dataInicio, [Required] DateTime dataFim)
        {
            var data = await _service.GetPorPeriodo(dataInicio, dataFim);
            return Ok(data);
        }
    }
}
