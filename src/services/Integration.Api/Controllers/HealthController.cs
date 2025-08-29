using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Integration.Api.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Redireciona para o Swagger quando acessar a raiz da API
        /// </summary>
        /// <returns>Redirecionamento para Swagger</returns>
        [HttpGet("/")]
        [AllowAnonymous]
        public IActionResult Root()
        {
            return Redirect("/swagger");
        }

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
            var domain = HttpContext.Request.Host.ToString();
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                port = Environment.GetEnvironmentVariable("PORT") ?? "80",
                domain = domain,
                swaggerUrl = $"{(HttpContext.Request.IsHttps ? "https" : "http")}://{domain}/swagger"
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
