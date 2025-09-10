using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Integration.Domain.Http.Response;
using Integration.Domain.Common;
using Integration.Service.Services;

namespace Integration.Api.Controllers
{
    [Route("api-integration/dashboard")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly DashboardService _service;

        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna os totais principais do dashboard
        /// </summary>
        /// <response code="200">Totais que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("totais")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<DashboardTotaisResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetTotais()
        {
            var data = await _service.GetTotais();
            return Ok(data);
        }

        /// <summary>
        /// Retorna a quantidade de consultas de hoje
        /// </summary>
        /// <response code="200">Quantidade que foi retornada com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("consultas-hoje/quantidade")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetQuantidadeConsultasHoje()
        {
            var totais = await _service.GetTotais() as DashboardTotaisResponse;
            return Ok(totais?.ConsultasHoje ?? 0);
        }

        /// <summary>
        /// Retorna a lista de consultas de hoje
        /// </summary>
        /// <response code="200">Consultas que foram retornadas com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("consultas-hoje")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ConsultaHojeResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetConsultasHoje()
        {
            var data = await _service.GetConsultasHoje();
            return Ok(data);
        }

        /// <summary>
        /// Retorna o status rápido do dashboard
        /// </summary>
        /// <response code="200">Status que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("status-rapido")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<StatusRapidoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetStatusRapido()
        {
            var data = await _service.GetStatusRapido();
            return Ok(data);
        }

        /// <summary>
        /// Retorna a lista de pacientes recentes
        /// </summary>
        /// <response code="200">Pacientes que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("pacientes-recentes")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PacienteRecenteResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetPacientesRecentes()
        {
            var data = await _service.GetPacientesRecentes();
            return Ok(data);
        }

        /// <summary>
        /// Retorna a quantidade total de pacientes
        /// </summary>
        /// <response code="200">Quantidade que foi retornada com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("pacientes/total")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetTotalPacientes()
        {
            var totais = await _service.GetTotais() as DashboardTotaisResponse;
            return Ok(totais?.TotalPacientes ?? 0);
        }
    }
}
