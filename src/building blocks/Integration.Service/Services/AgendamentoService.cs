using AutoMapper;
using FluentValidator;
using Integration.Domain.Entities;
using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Repositories;
using Integration.Domain.Common;
using Integration.Domain.Enums;
using Integration.Infrastructure.Transactions;

namespace Integration.Service.Services
{
    public class AgendamentoService : Notifiable
    {
        private readonly IAgendamentoRepository _repository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IProfissionalRepository _profissionalRepository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public AgendamentoService(IAgendamentoRepository repository, IPacienteRepository pacienteRepository,
            IProfissionalRepository profissionalRepository, IMapper mapper, IUow uow)
        {
            _repository = repository;
            _pacienteRepository = pacienteRepository;
            _profissionalRepository = profissionalRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ICommandResult> Handle(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Alert", "Agendamento não encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<AgendamentoResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> Listar()
        {
            var entities = await _repository.GetListDataAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhum agendamento encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<AgendamentoResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetAgendamentosHoje()
        {
            var entities = await _repository.GetAgendamentosHojeAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhum agendamento para hoje");

            if (!IsValid()) return default;

            return _mapper.Map<List<AgendamentoResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorProfissional(Guid profissionalId)
        {
            var entities = await _repository.GetPorProfissionalAsync(profissionalId);

            if (!entities.Any()) AddNotification("Alert", "Nenhum agendamento encontrado para este profissional");

            if (!IsValid()) return default;

            return _mapper.Map<List<AgendamentoResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorData(DateTime data)
        {
            var entities = await _repository.GetPorDataAsync(data);

            if (!entities.Any()) AddNotification("Alert", "Nenhum agendamento encontrado para esta data");

            if (!IsValid()) return default;

            return _mapper.Map<List<AgendamentoResponse>>(entities);
        }

        public async Task<ICommandResult> Handle(AgendamentoRegisterRequest request)
        {
            // Verificar se profissional existe
            var profissional = await _profissionalRepository.GetDataAsync(x => x.Id == request.ProfissionalId);
            if (profissional is null)
                AddNotification("Warning", "Profissional não encontrado");

            // Verificar disponibilidade
            var disponivel = await _repository.VerificarDisponibilidadeAsync(request.ProfissionalId,
                request.DataAgendamento, request.HorarioInicio, request.DuracaoMinutos);

            if (!disponivel)
                AddNotification("Warning", "Horário não disponível");

            if (!IsValid()) return default;

            var entity = new Agendamento(default, request.ProfissionalId, request.PacienteNome,
                request.DataAgendamento, request.HorarioInicio, request.Servico);

            AddNotifications(entity.Notifications);

            if (!IsValid()) return default;

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<AgendamentoResponse>(entity);
        }

        public async Task<ICommandResult> Handle(ReagendarRequest request)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == request.Id);

            if (entity is null) AddNotification("Warning", "Agendamento não encontrado");

            // Verificar nova disponibilidade
            var disponivel = await _repository.VerificarDisponibilidadeAsync(entity.ProfissionalId,
                request.NovaDataAgendamento, request.NovoHorarioInicio, entity.DuracaoMinutos);

            if (!disponivel)
                AddNotification("Warning", "Novo horário não disponível");

            if (!IsValid()) return default;

            entity.Reagendar(request.NovaDataAgendamento, request.NovoHorarioInicio);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<AgendamentoResponse>(entity);
        }

        public async Task<ICommandResult> Handle(CancelarAgendamentoRequest request)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == request.Id);

            if (entity is null) AddNotification("Warning", "Agendamento não encontrado");

            if (!IsValid()) return default;

            entity.AtualizarStatus(StatusAgendamento.Cancelado, request.ObservacoesCancelamento);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<AgendamentoResponse>(entity);
        }

        public async Task<ICommandResult> AlterarStatus(Guid id, StatusAgendamento novoStatus)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Agendamento não encontrado");

            if (!IsValid()) return default;

            entity.AtualizarStatus(novoStatus);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<AgendamentoResponse>(entity);
        }

        public async Task<ICommandResult> Delete(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Agendamento não encontrado");

            if (!IsValid()) return default;

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return _mapper.Map<AgendamentoResponse>(entity);
        }
    }
}
