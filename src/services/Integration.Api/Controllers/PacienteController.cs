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
    [Route("api-integration/paciente")]
    [ApiController]
    public class PacienteController : BaseController
    {
        private readonly PacienteService _service;

        public PacienteController(PacienteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna o paciente filtrado pelo id
        /// </summary>
        /// <param name="id">ID do paciente</param>
        /// <response code="200">Paciente que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<PacienteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            var data = await _service.Handle(id);
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista de todos os pacientes
        /// </summary>
        /// <response code="200">Pacientes que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PacienteResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.Listar();
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista de pacientes ativos
        /// </summary>
        /// <response code="200">Pacientes ativos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("ativos")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PacienteResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAtivos()
        {
            var data = await _service.ListarAtivos();
            return Ok(data);
        }

        /// <summary>
        /// Retorna o paciente filtrado pelo CPF
        /// </summary>
        /// <param name="cpf">CPF do paciente (somente números)</param>
        /// <response code="200">Paciente que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("cpf/{cpf}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<PacienteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByCpf([Required] string cpf)
        {
            var data = await _service.GetByCpf(cpf);
            return Ok(data);
        }

        /// <summary>
        /// Retorna pacientes filtrados pelo nome (busca parcial)
        /// </summary>
        /// <param name="nome">Nome ou parte do nome do paciente</param>
        /// <response code="200">Pacientes que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("nome/{nome}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PacienteResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetByNome([Required] string nome)
        {
            var data = await _service.GetByNome(nome);
            return Ok(data);
        }

        /// <summary>
        /// Busca pacientes por CPF ou nome (busca unificada)
        /// </summary>
        /// <param name="termo">Termo de busca (CPF ou nome)</param>
        /// <response code="200">Pacientes que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("buscar/{termo}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PacienteResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> BuscarPorCpfOuNome([Required] string termo)
        {
            var data = await _service.GetByCpfOrNome(termo);
            return Ok(data);
        }

        /// <summary>
        /// Cadastra um novo paciente
        /// </summary>
        /// <param name="request">Dados do paciente para cadastro</param>
        /// <response code="200">Paciente que foi inserido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<PacienteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Create([FromBody] PacienteRegisterRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Atualiza um paciente existente
        /// </summary>
        /// <param name="request">Dados do paciente para atualização</param>
        /// <response code="200">Paciente que foi alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPut]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<PacienteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Update([FromBody] PacienteUpdateRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Altera o status de um paciente
        /// </summary>
        /// <param name="id">ID do paciente</param>
        /// <param name="status">Novo status do paciente</param>
        /// <response code="200">Status do paciente alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPatch("{id:guid}/status")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<PacienteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> AlterarStatus([Required] Guid id, [FromBody] StatusPaciente status)
        {
            var data = await _service.AlterarStatus(id, status);
            return Ok(data);
        }

        /// <summary>
        /// Remove um paciente (exclusão física)
        /// </summary>
        /// <param name="id">ID do paciente</param>
        /// <response code="200">Paciente que foi removido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpDelete("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<PacienteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Delete([Required] Guid id)
        {
            var data = await _service.Delete(id);
            return Ok(data);
        }
    }
}
