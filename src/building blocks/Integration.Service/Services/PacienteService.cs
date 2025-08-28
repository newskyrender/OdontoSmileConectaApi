using AutoMapper;
using FluentValidator;
using Integration.Domain.Entities;
using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Repositories;
using Integration.Domain.Common;
using Integration.Domain.Enums;
using Integration.Infrastructure.Transactions;
using System.Security.Cryptography;
using System.Text;

namespace Integration.Service.Services
{
    public class PacienteService : Notifiable
    {
        private readonly IPacienteRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public PacienteService(IPacienteRepository repository, IMapper mapper, IUow uow)
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ICommandResult> Handle(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Alert", "Paciente não encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<PacienteResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> Listar()
        {
            var entities = await _repository.GetListDataAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhum paciente encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<PacienteResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> ListarAtivos()
        {
            var entities = await _repository.GetPacientesAtivosAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhum paciente ativo encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<PacienteResponse>>(entities);
        }

        public async Task<ICommandResult> GetByCpf(string cpf)
        {
            var entity = await _repository.GetByCpfAsync(cpf);

            if (entity is null) AddNotification("Alert", "Paciente não encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<PacienteResponse>(entity);
        }

        public async Task<ICommandResult> Handle(PacienteRegisterRequest request)
        {
            // Verificar se CPF já existe
            if (await _repository.ExisteCpfAsync(request.Cpf))
                AddNotification("Warning", "CPF já cadastrado");

            if (!IsValid()) return default;

            var entity = new Paciente(default, request.NomeCompleto, request.Cpf, request.DataNascimento,
                request.EstadoCivil, request.Sexo, request.CelularPrincipal, request.Email,
                request.Cep, request.EnderecoCompleto, request.Bairro, request.Cidade, request.Estado);

            // Atualizar dados opcionais
            if (!string.IsNullOrEmpty(request.Profissao))
                entity.AtualizarDadosPessoais(request.NomeCompleto, request.DataNascimento,
                    request.EstadoCivil, request.Sexo, request.Profissao);

            if (!string.IsNullOrEmpty(request.TelefoneFixo) || !string.IsNullOrEmpty(request.ContatoEmergencia))
                entity.AtualizarContato(request.CelularPrincipal, request.TelefoneFixo,
                    request.Email, request.ContatoEmergencia);

            if (!string.IsNullOrEmpty(request.Complemento) || !string.IsNullOrEmpty(request.Numero))
                entity.AtualizarEndereco(request.Cep, request.EnderecoCompleto, request.Bairro,
                    request.Cidade, request.Estado, request.Complemento, request.Numero);

            if (!string.IsNullOrEmpty(request.NumeroCooperado))
                entity.AtualizarDadosCooperativa(request.NumeroCooperado, request.SituacaoCooperativa,
                    request.RendaMensal, request.LimiteDisponivel, request.LimiteTotal);

            AddNotifications(entity.Notifications);

            if (!IsValid()) return default;

            await _repository.AddAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<PacienteResponse>(entity);
        }

        public async Task<ICommandResult> Handle(PacienteUpdateRequest request)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == request.Id);

            if (entity is null) AddNotification("Warning", "Paciente não encontrado");

            // Verificar se CPF já existe para outro paciente
            var existeCpf = await _repository.GetByCpfAsync(request.Cpf);
            if (existeCpf != null && existeCpf.Id != request.Id)
                AddNotification("Warning", "CPF já cadastrado para outro paciente");

            if (!IsValid()) return default;

            // Atualizar dados
            entity.AtualizarDadosPessoais(request.NomeCompleto, request.DataNascimento,
                request.EstadoCivil, request.Sexo, request.Profissao);

            entity.AtualizarContato(request.CelularPrincipal, request.TelefoneFixo,
                request.Email, request.ContatoEmergencia);

            entity.AtualizarEndereco(request.Cep, request.EnderecoCompleto, request.Bairro,
                request.Cidade, request.Estado, request.Complemento, request.Numero);

            if (!string.IsNullOrEmpty(request.NumeroCooperado))
                entity.AtualizarDadosCooperativa(request.NumeroCooperado, request.SituacaoCooperativa,
                    request.RendaMensal, request.LimiteDisponivel, request.LimiteTotal);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<PacienteResponse>(entity);
        }

        public async Task<ICommandResult> Delete(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Paciente não encontrado");

            if (!IsValid()) return default;

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return _mapper.Map<PacienteResponse>(entity);
        }

        public async Task<ICommandResult> AlterarStatus(Guid id, StatusPaciente novoStatus)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Paciente não encontrado");

            if (!IsValid()) return default;

            entity.AlterarStatus(novoStatus);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<PacienteResponse>(entity);
        }
    }
}
