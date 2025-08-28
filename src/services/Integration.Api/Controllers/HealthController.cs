using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Integration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>Status da API</returns>
        /// <response code="200">API funcionando corretamente</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            });
        }

        /// <summary>
        /// Endpoint de teste
        /// </summary>
        /// <returns>Mensagem de teste</returns>
        /// <response code="200">Teste realizado com sucesso</response>
        [HttpGet("test")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult Test()
        {
            return Ok(new
            {
                message = "Integration API está funcionando!",
                timestamp = DateTime.UtcNow,
                requestId = Guid.NewGuid()
            });
        }
    }
}
