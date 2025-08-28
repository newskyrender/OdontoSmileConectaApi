using AutoMapper;
using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Http.Request;
using Integration.Domain.Http.Response;
using Integration.Domain.Repositories;
using Integration.Infrastructure.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service.Services
{
    public class DocumentoService : Notifiable
    {
        private readonly IDocumentoRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public DocumentoService(IDocumentoRepository repository, IMapper mapper, IUow uow)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ICommandResult> Handle(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Alert", "Documento não encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<DocumentoResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorEntidade(EntidadeTipo entidadeTipo, Guid entidadeId)
        {
            var entities = await _repository.GetPorEntidadeAsync(entidadeTipo, entidadeId);

            if (!entities.Any()) AddNotification("Alert", "Nenhum documento encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<DocumentoResponse>>(entities);
        }

        public async Task<ICommandResult> Handle(DocumentoUploadRequest request)
        {
            var entity = new Documento(default, request.EntidadeTipo, request.EntidadeId,
                request.TipoDocumento, request.NomeOriginal, request.NomeArquivo,
                request.CaminhoArquivo, request.TamanhoBytes, request.TipoMime);

            AddNotifications(entity.Notifications);

            if (!IsValid()) return default;

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<DocumentoResponse>(entity);
        }

        public async Task<ICommandResult> AlterarStatus(Guid id, StatusDocumento novoStatus)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Documento não encontrado");

            if (!IsValid()) return default;

            entity.AtualizarStatus(novoStatus);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<DocumentoResponse>(entity);
        }

        public async Task<ICommandResult> Delete(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Documento não encontrado");

            if (!IsValid()) return default;

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return _mapper.Map<DocumentoResponse>(entity);
        }
    }
}
