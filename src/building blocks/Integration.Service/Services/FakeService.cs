using AutoMapper;
using FluentValidator;
using Integration.Domain.Entities;
using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Repositories;
using Integration.Domain.Common;
using Integration.Infrastructure.Transactions;
// using Seven.Core.Lib.Commands; - Temporarily disabled

namespace Integration.Service.Services
{
    public class FakeService: Notifiable
                // ICommandHandler<FakeRegisterRequest>, - Temporarily disabled
                // ICommandHandler<FakeUpdateRequest>   - Temporarily disabled
    {
        private readonly IFakeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public FakeService(IFakeRepository fakeRepository, IMapper mapper, IUow uow)
        {
            _repository = fakeRepository;
            _mapper = mapper;
            _uow = uow;

        }

        public async Task<ICommandResult> Handle(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Alert", "Nenhum registro encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<FakeResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> Listar()
        {
            var entity = await _repository.GetListDataAsync();

            if (entity.Count() <= 0) AddNotification("Alert", "Nenhum registro encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<FakeResponse>>(entity);
        }

        public async Task<ICommandResult> Handle(FakeRegisterRequest request)
        {
            var entity = new Fake(default, request.Nome, request.Email);

            // AddNotifications(entity.Notifications); - Temporarily disabled

            if (!IsValid()) return default;

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<FakeResponse>(entity);
        }

        public async Task<ICommandResult> Handle(FakeUpdateRequest request)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == request.Id);

            if (entity is null) AddNotification("Warning", "Registro n�o encontrado");

            entity = new Fake(request.Id, request.Nome, request.Email);
           
            // AddNotifications(entity.Notifications); - Temporarily disabled

            if (!IsValid()) return default;

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<FakeResponse>(entity);
        }

        public async Task<ICommandResult> Delete(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Registro n�o encontrado");

            if (!IsValid()) return default;

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return _mapper.Map<FakeResponse>(entity);
        }

    }
}

