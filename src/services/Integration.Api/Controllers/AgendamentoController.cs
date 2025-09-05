using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Common;
using Integration.Domain.Enums;
using Integration.Service.Services;
using System.ComponentModel.DataAnnotations;
using Integration.Domain.Repositories;
using AutoMapper;

namespace Integration.Api.Controllers
{
    [Route("api-integration/agendamento")]
    [ApiController]
    public class AgendamentoController : BaseController
    {
        private readonly AgendamentoService _service;
        private readonly IAgendamentoRepository _repository;
        private readonly IMapper _mapper;

        public AgendamentoController(AgendamentoService service, IAgendamentoRepository repository, IMapper mapper)
        {
            _service = service;
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna o agendamento filtrado pelo id
        /// </summary>
        /// <param name="id">ID do agendamento</param>
        /// <response code="200">Agendamento que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<AgendamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            var data = await _service.Handle(id);
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista de todos os agendamentos
        /// </summary>
        /// <response code="200">Agendamentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AgendamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.Listar();
            return Ok(data);
        }

        /// <summary>
        /// Retorna os agendamentos do dia atual
        /// </summary>
        /// <response code="200">Agendamentos de hoje retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("hoje")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AgendamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetHoje()
        {
            var data = await _service.GetAgendamentosHoje();
            return Ok(data);
        }

        /// <summary>
        /// Retorna os agendamentos por profissional
        /// </summary>
        /// <param name="profissionalId">ID do profissional</param>
        /// <response code="200">Agendamentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("profissional/{profissionalId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AgendamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByProfissional([Required] Guid profissionalId)
        {
            var data = await _service.GetPorProfissional(profissionalId);
            return Ok(data);
        }

        /// <summary>
        /// Retorna os agendamentos para uma data específica
        /// </summary>
        /// <param name="data">Data de agendamento (yyyy-MM-dd)</param>
        /// <response code="200">Agendamentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("data/{data:datetime}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AgendamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByData([Required] DateTime data)
        {
            var agendamentos = await _service.GetPorData(data);
            return Ok(agendamentos);
        }

        /// <summary>
        /// Retorna os agendamentos por profissional e data
        /// </summary>
        /// <param name="profissionalId">ID do profissional</param>
        /// <param name="data">Data de agendamento (yyyy-MM-dd)</param>
        /// <response code="200">Agendamentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("profissional/{profissionalId:guid}/data/{data:datetime}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AgendamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByProfissionalEData([Required] Guid profissionalId, [Required] DateTime data)
        {
            // Como este método não existe no service, vamos usar o repositório diretamente
            var agendamentos = await _repository.GetPorProfissionalEDataAsync(profissionalId, data);
            var response = _mapper.Map<List<AgendamentoResponse>>(agendamentos);
            return Ok(response);
        }

        /// <summary>
        /// Retorna os agendamentos por paciente
        /// </summary>
        /// <param name="pacienteId">ID do paciente</param>
        /// <response code="200">Agendamentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("paciente/{pacienteId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AgendamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByPaciente([Required] Guid pacienteId)
        {
            var agendamentos = await _repository.GetPorPacienteAsync(pacienteId);
            var response = _mapper.Map<List<AgendamentoResponse>>(agendamentos);
            return Ok(response);
        }

        /// <summary>
        /// Retorna os agendamentos por status
        /// </summary>
        /// <param name="status">Status do agendamento (Agendado, Confirmado, Cancelado, Realizado, Remarcado)</param>
        /// <response code="200">Agendamentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("status/{status}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AgendamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByStatus([Required] StatusAgendamento status)
        {
            var agendamentos = await _repository.GetPorStatusAsync(status);
            var response = _mapper.Map<List<AgendamentoResponse>>(agendamentos);
            return Ok(response);
        }

        /// <summary>
        /// Retorna os agendamentos por período
        /// </summary>
        /// <param name="dataInicio">Data de início (yyyy-MM-dd)</param>
        /// <param name="dataFim">Data de fim (yyyy-MM-dd)</param>
        /// <response code="200">Agendamentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("periodo")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AgendamentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByPeriodo([Required] DateTime dataInicio, [Required] DateTime dataFim)
        {
            var agendamentos = await _repository.GetPorPeriodoAsync(dataInicio, dataFim);
            var response = _mapper.Map<List<AgendamentoResponse>>(agendamentos);
            return Ok(response);
        }

        /// <summary>
        /// Verifica disponibilidade para agendamento
        /// </summary>
        /// <param name="profissionalId">ID do profissional</param>
        /// <param name="data">Data do agendamento (yyyy-MM-dd)</param>
        /// <param name="horario">Horário de início (HH:mm)</param>
        /// <param name="duracao">Duração em minutos</param>
        /// <response code="200">Disponibilidade verificada com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("disponibilidade")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> VerificarDisponibilidade(
            [Required] Guid profissionalId, 
            [Required] DateTime data, 
            [Required] TimeSpan horario, 
            [Required] int duracao)
        {
            var disponivel = await _repository.VerificarDisponibilidadeAsync(profissionalId, data, horario, duracao);
            return Ok(disponivel);
        }

        /// <summary>
        /// Cadastra um novo agendamento
        /// </summary>
        /// <param name="request">Dados do agendamento para cadastro</param>
        /// <response code="200">Agendamento que foi inserido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<AgendamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Create([FromBody] AgendamentoRegisterRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Reagenda um agendamento existente
        /// </summary>
        /// <param name="request">Dados do reagendamento</param>
        /// <response code="200">Agendamento que foi reagendado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost("reagendar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<AgendamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Reagendar([FromBody] ReagendarRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Cancela um agendamento
        /// </summary>
        /// <param name="request">Dados para cancelamento do agendamento</param>
        /// <response code="200">Agendamento que foi cancelado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost("cancelar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<AgendamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Cancelar([FromBody] CancelarAgendamentoRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Altera o status de um agendamento
        /// </summary>
        /// <param name="id">ID do agendamento</param>
        /// <param name="status">Novo status do agendamento</param>
        /// <response code="200">Status do agendamento alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPatch("{id:guid}/status")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<AgendamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> AlterarStatus([Required] Guid id, [FromBody] StatusAgendamento status)
        {
            var data = await _service.AlterarStatus(id, status);
            return Ok(data);
        }

        /// <summary>
        /// Remove um agendamento (exclusão física)
        /// </summary>
        /// <param name="id">ID do agendamento</param>
        /// <response code="200">Agendamento que foi removido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpDelete("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<AgendamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Delete([Required] Guid id)
        {
            var data = await _service.Delete(id);
            return Ok(data);
        }
    }
}
