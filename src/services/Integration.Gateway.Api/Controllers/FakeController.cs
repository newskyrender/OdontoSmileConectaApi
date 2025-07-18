using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Common;
using Integration.Gateway.Api.Service;
using System.ComponentModel.DataAnnotations;

namespace Integration.Gateway.Api.Controllers
{
    public class FakeController: BaseController
    {
        private readonly FakeService _service;

        public FakeController(FakeService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna o registro filtrado pelo id
        /// </summary>
        /// <response code="200">Registro que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pre-condi��o ou um algum erro interno.</response>
        [HttpGet, Route("get-fake"), AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<FakeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Get([Required] Guid id)
        {
            var data = await _service.Get(id, Request.Headers["Authorization"]);
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista dos registros da tabela
        /// </summary>
        /// <response code="200">Registros que foram retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pre-condi��o ou um algum erro interno.</response>
        [HttpGet, Route("get-list-fake"), AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<FakeResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAll(Request.Headers["Authorization"]);
            return Ok(data);
        }

        /// <summary>
        /// Adiciona um registro na tabela fake
        /// </summary>
        /// <response code="200">Registro que foi inserido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pre-condi��o ou um algum erro interno.</response>
        [HttpPost, Route("add-fake"), AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<FakeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Post([FromBody] FakeRegisterRequest request)
        {
            var data = await _service.Add(request, Request.Headers["Authorization"]);
            return Ok(data);
        }

        /// <summary>
        /// Alterar um registro na tabela fake
        /// </summary>
        /// <response code="200">Registro que foi alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pre-condi��o ou um algum erro interno.</response>
        [HttpPut, Route("update-fake"), AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<FakeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Put([FromBody] FakeUpdateRequest request)
        {
            var data = await _service.Update(request, Request.Headers["Authorization"]);
            return Ok(data);
        }

        /// <summary>
        /// Deleta um registro na tabela fake
        /// </summary>
        /// <response code="200">Registro que foi deletado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pre-condi��o ou um algum erro interno.</response>
        [HttpDelete, Route("delete-fake"), AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<FakeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Delete([FromQuery, Required] Guid id)
        {
            var data = await _service.Delete(id, Request.Headers["Authorization"]);
            return Ok(data);
        }

    }
}
