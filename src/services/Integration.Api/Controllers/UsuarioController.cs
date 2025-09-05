using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Common;
using Integration.Service.Services;
using System.ComponentModel.DataAnnotations;
using Integration.Domain.Repositories;

namespace Integration.Api.Controllers
{
    [Route("api-integration/usuario")]
    [ApiController]
    public class UsuarioController : BaseController
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna o usuário filtrado pelo id
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <response code="200">Usuário que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            var data = await _service.Handle(id);
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista de todos os usuários
        /// </summary>
        /// <response code="200">Usuários que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<UsuarioResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.Listar();
            return Ok(data);
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="request">Dados do usuário para cadastro</param>
        /// <response code="200">Usuário que foi inserido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Create([FromBody] UsuarioRegisterRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="request">Dados do usuário para atualização</param>
        /// <response code="200">Usuário que foi atualizado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPut("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Update([Required] Guid id, [FromBody] UsuarioUpdateRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest(new ResponseError("Os IDs não coincidem"));
            }

            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Remove um usuário
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <response code="200">Usuário que foi removido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpDelete("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Delete([Required] Guid id)
        {
            var data = await _service.Delete(id);
            return Ok(data);
        }
        
        /// <summary>
        /// Ativa ou desativa um usuário
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="ativo">True para ativar, False para desativar</param>
        /// <response code="200">Status do usuário alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPatch("{id:guid}/status")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> AlterarStatus([Required] Guid id, [FromBody] bool ativo)
        {
            var usuario = await _service.Handle(id);
            if (usuario == null)
                return BadRequest(new ResponseError("Usuário não encontrado"));

            // Como não temos um método específico no service para alteração de status,
            // vamos usar o método Update com todos os dados atuais e apenas alterando o status
            var usuarioAtual = usuario as UsuarioResponse;
            var request = new UsuarioUpdateRequest
            {
                Id = id,
                Email = usuarioAtual.Email,
                Nome = usuarioAtual.Nome,
                Ativo = ativo,
                // Não incluímos a senha aqui para evitar alteração
            };

            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Realiza o login do usuário
        /// </summary>
        /// <param name="request">Dados para login</param>
        /// <response code="200">Login realizado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Verifica se um email já está cadastrado
        /// </summary>
        /// <param name="email">Email a ser verificado</param>
        /// <response code="200">Verificação realizada com sucesso.</response>
        [HttpGet("verificar-email")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> VerificarEmail([Required][FromQuery] string email)
        {
            var repository = HttpContext.RequestServices.GetService<IUsuarioRepository>();
            var existe = await repository.ExisteEmailAsync(email);
            return Ok(existe);
        }
        
        /// <summary>
        /// Altera a senha de um usuário
        /// </summary>
        /// <param name="request">Dados para alteração de senha</param>
        /// <response code="200">Senha alterada com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost("alterar-senha")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }
    }
}
