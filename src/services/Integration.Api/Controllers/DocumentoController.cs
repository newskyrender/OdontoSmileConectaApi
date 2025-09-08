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
    [Route("api-integration/documento")]
    [ApiController]
    public class DocumentoController : BaseController
    {
        private readonly DocumentoService _service;

        public DocumentoController(DocumentoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retorna o documento filtrado pelo id
        /// </summary>
        /// <param name="id">ID do documento</param>
        /// <response code="200">Documento que foi retornado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<DocumentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            var data = await _service.Handle(id);
            return Ok(data);
        }

        /// <summary>
        /// Lista todos os documentos
        /// </summary>
        /// <response code="200">Documentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<DocumentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.Listar();
            return Ok(data);
        }
        
        /// <summary>
        /// Retorna documentos por entidade
        /// </summary>
        /// <param name="entidadeTipo">Tipo da entidade (Profissional, Paciente, Orcamento)</param>
        /// <param name="entidadeId">ID da entidade</param>
        /// <response code="200">Documentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("entidade/{entidadeTipo}/{entidadeId:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<DocumentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetPorEntidade([Required] EntidadeTipo entidadeTipo, [Required] Guid entidadeId)
        {
            var data = await _service.GetPorEntidade(entidadeTipo, entidadeId);
            return Ok(data);
        }
        
        /// <summary>
        /// Retorna documentos por tipo de documento
        /// </summary>
        /// <param name="tipoDocumento">Tipo de documento</param>
        /// <response code="200">Documentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("tipo/{tipoDocumento}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<DocumentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetPorTipo([Required] TipoDocumento tipoDocumento)
        {
            var data = await _service.GetPorTipo(tipoDocumento);
            return Ok(data);
        }
        
        /// <summary>
        /// Retorna documentos por status
        /// </summary>
        /// <param name="status">Status do documento</param>
        /// <response code="200">Documentos que foram retornados com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("status/{status}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<DocumentoResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> GetPorStatus([Required] StatusDocumento status)
        {
            var data = await _service.GetPorStatus(status);
            return Ok(data);
        }

        /// <summary>
        /// Upload de um novo documento (dados informados manualmente)
        /// </summary>
        /// <param name="request">Dados do documento para upload</param>
        /// <response code="200">Documento que foi inserido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<DocumentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Upload([FromBody] DocumentoUploadRequest request)
        {
            var data = await _service.Handle(request);
            return Ok(data);
        }
        
        /// <summary>
        /// Upload de arquivo físico
        /// </summary>
        /// <param name="entidadeTipo">Tipo da entidade</param>
        /// <param name="entidadeId">ID da entidade</param>
        /// <param name="tipoDocumento">Tipo do documento</param>
        /// <param name="file">Arquivo a ser enviado</param>
        /// <response code="200">Documento que foi inserido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPost("upload-arquivo")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<DocumentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> UploadFile([FromForm] EntidadeTipo entidadeTipo, [FromForm] Guid entidadeId, 
            [FromForm] TipoDocumento tipoDocumento, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ResponseError("Nenhum arquivo enviado"));

            // Criar pasta para armazenar o arquivo se não existir
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Gerar um nome único para o arquivo
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Salvar o arquivo
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Criar o request
            var request = new DocumentoUploadRequest
            {
                EntidadeTipo = entidadeTipo,
                EntidadeId = entidadeId,
                TipoDocumento = tipoDocumento,
                NomeOriginal = file.FileName,
                NomeArquivo = fileName,
                CaminhoArquivo = filePath,
                TamanhoBytes = (int)file.Length,
                TipoMime = file.ContentType
            };

            // Salvar no banco
            var data = await _service.Handle(request);
            return Ok(data);
        }

        /// <summary>
        /// Atualiza o status de um documento
        /// </summary>
        /// <param name="id">ID do documento</param>
        /// <param name="status">Novo status do documento</param>
        /// <response code="200">Status do documento alterado com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpPatch("{id:guid}/status")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<DocumentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> AlterarStatus([Required] Guid id, [FromBody] StatusDocumento status)
        {
            var data = await _service.AlterarStatus(id, status);
            return Ok(data);
        }

        /// <summary>
        /// Download de um documento
        /// </summary>
        /// <param name="id">ID do documento</param>
        /// <response code="200">Arquivo para download.</response>
        /// <response code="404">Documento não encontrado.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpGet("{id:guid}/download")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Download([Required] Guid id)
        {
            var documento = await _service.Handle(id) as DocumentoResponse;
            
            if (documento == null)
                return NotFound(new ResponseError("Documento não encontrado"));
                
            if (!System.IO.File.Exists(documento.CaminhoArquivo))
                return NotFound(new ResponseError("Arquivo físico não encontrado"));
                
            var fileBytes = await System.IO.File.ReadAllBytesAsync(documento.CaminhoArquivo);
            
            return File(fileBytes, documento.TipoMime, documento.NomeOriginal);
        }

        /// <summary>
        /// Remove um documento
        /// </summary>
        /// <param name="id">ID do documento</param>
        /// <response code="200">Documento que foi removido com sucesso.</response>
        /// <response code="412">Ocorreu uma falha de pré-condição ou um algum erro interno.</response>
        [HttpDelete("{id:guid}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<DocumentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseError), StatusCodes.Status412PreconditionFailed)]
        public async Task<IActionResult> Delete([Required] Guid id)
        {
            var documento = await _service.Handle(id) as DocumentoResponse;
            
            if (documento != null && System.IO.File.Exists(documento.CaminhoArquivo))
            {
                try
                {
                    System.IO.File.Delete(documento.CaminhoArquivo);
                }
                catch
                {
                    // Ignora erro ao excluir arquivo físico
                }
            }
            
            var data = await _service.Delete(id);
            return Ok(data);
        }
    }
}
