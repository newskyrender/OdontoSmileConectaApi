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
    [Route("api-integration/profissional")]
    [ApiController]
    public class ProfissionalController : BaseController
    {
        private readonly ProfissionalService _service;

        public ProfissionalController(ProfissionalService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna o profissional filtrado pelo id
        /// </summary>
        /// <param name="id">ID do profissional</param>
        /// <response code="200">Profissional que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<ProfissionalResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            var data = await _service.Handle(id);
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista de todos os profissionais
        /// </summary>
        /// <response code="200">Profissionais que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ProfissionalResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.Listar();
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista de profissionais ativos
        /// </summary>
        /// <response code="200">Profissionais ativos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("ativos")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ProfissionalResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAtivos()
        {
            var data = await _service.ListarAtivos();
            return Ok(data);
        }

        /// <summary>
        /// Retorna os profissionais por status de aprovação
        /// </summary>
        /// <param name="status">Status de aprovação (Pendente, Aprovado, Reprovado)</param>
        /// <response code="200">Profissionais que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("status/{status}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ProfissionalResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByStatus([Required] StatusAprovacao status)
        {
            var data = await _service.GetPorStatusAprovacao(status);
            return Ok(data);
        }

        /// <summary>
        /// Retorna os profissionais por especialidade
        /// </summary>
        /// <param name="especialidade">Especialidade do profissional</param>
        /// <response code="200">Profissionais que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("especialidade/{especialidade}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ProfissionalResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByEspecialidade([Required] Especialidade especialidade)
        {
            var data = await _service.GetPorEspecialidade(especialidade);
            return Ok(data);
        }

        /// <summary>
        /// Retorna o profissional filtrado pelo CPF
        /// </summary>
        /// <param name="cpf">CPF do profissional (somente números)</param>
        /// <response code="200">Profissional que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("cpf/{cpf}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<ProfissionalResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByCpf([Required] string cpf)
        {
            var data = await _service.GetByCpf(cpf);
            return Ok(data);
        }

        /// <summary>
        /// Retorna o profissional filtrado pelo CRO
        /// </summary>
        /// <param name="cro">CRO do profissional</param>
        /// <response code="200">Profissional que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("cro/{cro}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<ProfissionalResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByCro([Required] string cro)
        {
            var data = await _service.GetByCro(cro);
            return Ok(data);
        }

        /// <summary>
        /// Cadastra um novo profissional
        /// </summary>
        /// <param name="request">Dados do profissional para cadastro</param>
        /// <response code="200">Profissional que foi inserido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<ProfissionalResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Create([FromBody] ProfissionalRegisterRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Atualiza um profissional existente
        /// </summary>
        /// <param name="request">Dados do profissional para atualização</param>
        /// <response code="200">Profissional que foi alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPut]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<ProfissionalResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Update([FromBody] ProfissionalUpdateRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Altera o status de aprovação de um profissional
        /// </summary>
        /// <param name="id">ID do profissional</param>
        /// <param name="status">Novo status de aprovação do profissional</param>
        /// <response code="200">Status do profissional alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPatch("{id:guid}/status-aprovacao")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<ProfissionalResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> AlterarStatusAprovacao([Required] Guid id, [FromBody] StatusAprovacao status)
        {
            var data = await _service.AlterarStatusAprovacao(id, status);
            return Ok(data);
        }

        /// <summary>
        /// Remove um profissional (exclusão física)
        /// </summary>
        /// <param name="id">ID do profissional</param>
        /// <response code="200">Profissional que foi removido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpDelete("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<ProfissionalResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Delete([Required] Guid id)
        {
            var data = await _service.Delete(id);
            return Ok(data);
        }
    }
}
