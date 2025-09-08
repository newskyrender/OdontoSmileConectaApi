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
    public class SolicitacaoOrcamentoService : Notifiable
    {
        private readonly ISolicitacaoOrcamentoRepository _repository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public SolicitacaoOrcamentoService(ISolicitacaoOrcamentoRepository repository,
            IPacienteRepository pacienteRepository, IMapper mapper, IUow uow)
        {
            _repository = repository;
            _pacienteRepository = pacienteRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ICommandResult> Handle(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Alert", "Solicitação não encontrada");

            if (!IsValid()) return default;

            return _mapper.Map<SolicitacaoOrcamentoResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> Listar()
        {
            var entities = await _repository.GetListDataAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhuma solicitação encontrada");

            if (!IsValid()) return default;

            return _mapper.Map<List<SolicitacaoOrcamentoResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorStatus(StatusSolicitacao status)
        {
            var entities = await _repository.GetPorStatusAsync(status);

            if (!entities.Any()) AddNotification("Alert", "Nenhuma solicitação encontrada com este status");

            if (!IsValid()) return default;

            return _mapper.Map<List<SolicitacaoOrcamentoResponse>>(entities);
        }

        public async Task<ICommandResult> Handle(SolicitacaoOrcamentoRegisterRequest request)
        {
            // Gerar número do pedido
            var numeroPedido = await _repository.GerarProximoNumeroPedidoAsync();

            var entity = new SolicitacaoOrcamento(default, numeroPedido, request.NomeCompleto,
                request.Cpf, request.Telefone, request.TipoTratamento);

            // Verificar se existe paciente com este CPF
            var paciente = await _pacienteRepository.GetByCpfAsync(request.Cpf);
            if (paciente != null)
            {
                // Usar dados do paciente cadastrado
                entity = new SolicitacaoOrcamento(default, numeroPedido, paciente.NomeCompleto,
                    paciente.Cpf, paciente.CelularPrincipal, request.TipoTratamento);
            }

            AddNotifications(entity.Notifications);

            if (!IsValid()) return default;

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<SolicitacaoOrcamentoResponse>(entity);
        }

        public async Task<ICommandResult> Handle(AprovarOrcamentoRequest request)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == request.Id);

            if (entity is null) AddNotification("Warning", "Solicitação não encontrada");

            if (entity.Status != StatusSolicitacao.Pendente && entity.Status != StatusSolicitacao.EmAnalise)
                AddNotification("Warning", "Solicitação não pode ser aprovada no status atual");

            if (!IsValid()) return default;

            entity.AprovarOrcamento(request.ValorAprovado, request.NumeroParcelas, request.ValorParcela);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<SolicitacaoOrcamentoResponse>(entity);
        }

        public async Task<ICommandResult> AlterarStatus(Guid id, StatusSolicitacao novoStatus)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Solicitação não encontrada");

            if (!IsValid()) return default;

            entity.AtualizarStatus(novoStatus);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<SolicitacaoOrcamentoResponse>(entity);
        }

        public async Task<ICommandResult> Delete(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Solicitação não encontrada");

            if (!IsValid()) return default;

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return _mapper.Map<SolicitacaoOrcamentoResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorPaciente(Guid pacienteId)
        {
            var entities = await _repository.GetPorPacienteAsync(pacienteId);

            if (!entities.Any()) AddNotification("Alert", "Nenhuma solicitação encontrada para este paciente");

            if (!IsValid()) return default;

            return _mapper.Map<List<SolicitacaoOrcamentoResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorProfissional(Guid profissionalId)
        {
            var entities = await _repository.GetPorProfissionalAsync(profissionalId);

            if (!entities.Any()) AddNotification("Alert", "Nenhuma solicitação encontrada para este profissional");

            if (!IsValid()) return default;

            return _mapper.Map<List<SolicitacaoOrcamentoResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorTipoTratamento(TipoTratamento tipoTratamento)
        {
            var entities = await _repository.GetPorTipoTratamentoAsync(tipoTratamento);

            if (!entities.Any()) AddNotification("Alert", $"Nenhuma solicitação encontrada para o tratamento {tipoTratamento}");

            if (!IsValid()) return default;

            return _mapper.Map<List<SolicitacaoOrcamentoResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            var entities = await _repository.GetPorPeriodoAsync(dataInicio, dataFim);

            if (!entities.Any()) AddNotification("Alert", "Nenhuma solicitação encontrada neste período");

            if (!IsValid()) return default;

            return _mapper.Map<List<SolicitacaoOrcamentoResponse>>(entities);
        }
    }
}
