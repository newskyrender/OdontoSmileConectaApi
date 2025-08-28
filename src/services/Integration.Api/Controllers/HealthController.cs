using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Integration.Api.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Health check endpoint para Railway
        /// </summary>
        /// <returns>Status da API</returns>
        /// <response code="200">API funcionando corretamente</response>
        [HttpGet("/health")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                port = Environment.GetEnvironmentVariable("PORT") ?? "80"
            });
        }

        /// <summary>
        /// Health check endpoint alternativo
        /// </summary>
        /// <returns>Status da API</returns>
        /// <response code="200">API funcionando corretamente</response>
        [HttpGet("api/health")]
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
        [HttpGet("api/health/test")]
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
