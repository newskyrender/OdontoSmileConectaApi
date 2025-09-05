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
    public class ProfissionalService : Notifiable
    {
        private readonly IProfissionalRepository _repository;
        private readonly IProfissionalEspecialidadeRepository _especialidadeRepository;
        private readonly IProfissionalEquipamentoRepository _equipamentoRepository;
        private readonly IProfissionalFacilidadeRepository _facilidadeRepository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public ProfissionalService(IProfissionalRepository repository,
            IProfissionalEspecialidadeRepository especialidadeRepository,
            IProfissionalEquipamentoRepository equipamentoRepository,
            IProfissionalFacilidadeRepository facilidadeRepository,
            IMapper mapper, IUow uow)
        {
            _repository = repository;
            _especialidadeRepository = especialidadeRepository;
            _equipamentoRepository = equipamentoRepository;
            _facilidadeRepository = facilidadeRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ICommandResult> GetByCpf(string cpf)
        {
            var entity = await _repository.GetByCpfAsync(cpf);

            if (entity is null) AddNotification("Alert", "Profissional não encontrado para o CPF informado");

            if (!IsValid()) return default;

            return _mapper.Map<ProfissionalResponse>(entity);
        }

        public async Task<ICommandResult> GetByCro(string cro)
        {
            var entity = await _repository.GetByCroAsync(cro);

            if (entity is null) AddNotification("Alert", "Profissional não encontrado para o CRO informado");

            if (!IsValid()) return default;

            return _mapper.Map<ProfissionalResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorStatusAprovacao(StatusAprovacao status)
        {
            var entities = await _repository.GetPorStatusAprovacaoAsync(status);

            if (!entities.Any()) AddNotification("Alert", $"Nenhum profissional encontrado com status {status}");

            if (!IsValid()) return default;

            return _mapper.Map<List<ProfissionalResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> GetPorEspecialidade(Especialidade especialidade)
        {
            var entities = await _repository.GetPorEspecialidadeAsync(especialidade);

            if (!entities.Any()) AddNotification("Alert", $"Nenhum profissional encontrado com a especialidade {especialidade}");

            if (!IsValid()) return default;

            return _mapper.Map<List<ProfissionalResponse>>(entities);
        }

        public async Task<ICommandResult> Handle(Guid id)
        {
            var entity = await _repository.GetCompletoAsync(id);

            if (entity is null) AddNotification("Alert", "Profissional não encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<ProfissionalResponse>(entity);
        }

        public async Task<IEnumerable<ICommandResult>> Listar()
        {
            var entities = await _repository.GetListDataAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhum profissional encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<ProfissionalResponse>>(entities);
        }

        public async Task<IEnumerable<ICommandResult>> ListarAtivos()
        {
            var entities = await _repository.GetProfissionaisAtivosAsync();

            if (!entities.Any()) AddNotification("Alert", "Nenhum profissional ativo encontrado");

            if (!IsValid()) return default;

            return _mapper.Map<List<ProfissionalResponse>>(entities);
        }

        public async Task<ICommandResult> Handle(ProfissionalRegisterRequest request)
        {
            // Verificações de duplicidade
            if (await _repository.ExisteCpfAsync(request.Cpf))
                AddNotification("Warning", "CPF já cadastrado");

            if (await _repository.ExisteCroAsync(request.Cro))
                AddNotification("Warning", "CRO já cadastrado");

            if (await _repository.ExisteEmailAsync(request.EmailProfissional))
                AddNotification("Warning", "E-mail já cadastrado");

            if (!IsValid()) return default;

            var entity = new Profissional(default, request.NomeCompleto, request.Cpf, request.DataNascimento,
                request.Sexo, request.EmailProfissional, request.Celular, request.Cro, request.DataFormatura,
                request.UniversidadeFormacao, request.TempoExperiencia);

            // Atualizar dados do consultório
            entity.AtualizarDadosConsultorio(request.NomeConsultorio, request.Cnpj, request.TelefoneConsultorio,
                request.CepConsultorio, request.EnderecoConsultorio, request.BairroConsultorio,
                request.CidadeConsultorio, request.EstadoConsultorio, request.ComplementoConsultorio,
                request.NumeroConsultorio, request.NumeroCadeiras);

            // Atualizar horários
            entity.AtualizarHorariosFuncionamento(request.SegundaSextaInicio, request.SegundaSextaFim,
                request.SabadoInicio, request.SabadoFim, request.DomingoInicio, request.DomingoFim,
                request.TempoMedioConsulta);

            // Atualizar dados bancários
            if (request.TipoConta.HasValue)
                entity.AtualizarDadosBancarios(request.Banco, request.TipoConta.Value, request.Agencia,
                    request.Conta, request.NomeTitular, request.CpfTitular);

            // Aceitar termos
            entity.AceitarTermos(request.TermosUso, request.CodigoEtica, request.Responsabilidade,
                request.DadosPessoais, request.Marketing);

            AddNotifications(entity.Notifications);

            if (!IsValid()) return default;

            await _repository.AddAsync(entity);

            // Adicionar especialidades
            foreach (var especialidade in request.Especialidades)
            {
                var profEspecialidade = new ProfissionalEspecialidade(entity.Id, especialidade);
                await _especialidadeRepository.AddAsync(profEspecialidade);
            }

            // Adicionar equipamentos
            foreach (var equipamento in request.Equipamentos)
            {
                var profEquipamento = new ProfissionalEquipamento(entity.Id, equipamento);
                await _equipamentoRepository.AddAsync(profEquipamento);
            }

            // Adicionar facilidades
            foreach (var facilidade in request.Facilidades)
            {
                var profFacilidade = new ProfissionalFacilidade(entity.Id, facilidade);
                await _facilidadeRepository.AddAsync(profFacilidade);
            }

            await _uow.CommitAsync();

            return _mapper.Map<ProfissionalResponse>(entity);
        }

        public async Task<ICommandResult> AlterarStatusAprovacao(Guid id, StatusAprovacao novoStatus)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Profissional não encontrado");

            if (!IsValid()) return default;

            entity.AlterarStatusAprovacao(novoStatus);

            await _repository.UpdateAsync(entity);
            await _uow.CommitAsync();

            return _mapper.Map<ProfissionalResponse>(entity);
        }

        public async Task<ICommandResult> Delete(Guid id)
        {
            var entity = await _repository.GetDataAsync(x => x.Id == id);

            if (entity is null) AddNotification("Warning", "Profissional não encontrado");

            if (!IsValid()) return default;

            _repository.Delete(entity);
            await _uow.CommitAsync();

            return _mapper.Map<ProfissionalResponse>(entity);
        }

        public async Task<ICommandResult> Handle(ProfissionalUpdateRequest request)
        {
            // Verificar se o profissional existe
            var entity = await _repository.GetCompletoAsync(request.Id);

            if (entity is null)
            {
                AddNotification("Warning", "Profissional não encontrado");
                return default;
            }

            // Verificações de duplicidade para CPF, CRO e email
            var profByCpf = await _repository.GetByCpfAsync(request.Cpf);
            if (profByCpf != null && profByCpf.Id != request.Id)
                AddNotification("Warning", "CPF já cadastrado para outro profissional");

            var profByCro = await _repository.GetByCroAsync(request.Cro);
            if (profByCro != null && profByCro.Id != request.Id)
                AddNotification("Warning", "CRO já cadastrado para outro profissional");

            if (!IsValid()) return default;

            // Aqui seria necessário criar métodos adicionais na entidade Profissional para 
            // atualizar os campos básicos que não têm métodos específicos.
            // Como não temos acesso para editar a classe Profissional, vamos usar os métodos existentes.

            // Atualizar dados do consultório
            entity.AtualizarDadosConsultorio(request.NomeConsultorio, request.Cnpj, request.TelefoneConsultorio,
                request.CepConsultorio, request.EnderecoConsultorio, request.BairroConsultorio,
                request.CidadeConsultorio, request.EstadoConsultorio, request.ComplementoConsultorio,
                request.NumeroConsultorio, request.NumeroCadeiras);

            // Atualizar horários
            entity.AtualizarHorariosFuncionamento(request.SegundaSextaInicio, request.SegundaSextaFim,
                request.SabadoInicio, request.SabadoFim, request.DomingoInicio, request.DomingoFim,
                request.TempoMedioConsulta);

            // Atualizar dados bancários
            if (request.TipoConta.HasValue)
                entity.AtualizarDadosBancarios(request.Banco, request.TipoConta.Value, request.Agencia,
                    request.Conta, request.NomeTitular, request.CpfTitular);

            // Aceitar termos
            entity.AceitarTermos(request.TermosUso, request.CodigoEtica, request.Responsabilidade,
                request.DadosPessoais, request.Marketing);

            // Salvar no banco
            await _repository.UpdateAsync(entity);

            // Atualizar especialidades (remover todas e adicionar novamente)
            var especialidadesAtuais = await _especialidadeRepository.GetListDataAsync(e => e.ProfissionalId == entity.Id);
            foreach (var especialidade in especialidadesAtuais)
            {
                _especialidadeRepository.Delete(especialidade);
            }
            
            foreach (var especialidade in request.Especialidades)
            {
                var profEspecialidade = new ProfissionalEspecialidade(entity.Id, especialidade);
                await _especialidadeRepository.AddAsync(profEspecialidade);
            }

            // Atualizar equipamentos (remover todos e adicionar novamente)
            var equipamentosAtuais = await _equipamentoRepository.GetListDataAsync(e => e.ProfissionalId == entity.Id);
            foreach (var equipamento in equipamentosAtuais)
            {
                _equipamentoRepository.Delete(equipamento);
            }
            
            foreach (var equipamento in request.Equipamentos)
            {
                var profEquipamento = new ProfissionalEquipamento(entity.Id, equipamento);
                await _equipamentoRepository.AddAsync(profEquipamento);
            }

            // Atualizar facilidades (remover todas e adicionar novamente)
            var facilidadesAtuais = await _facilidadeRepository.GetListDataAsync(f => f.ProfissionalId == entity.Id);
            foreach (var facilidade in facilidadesAtuais)
            {
                _facilidadeRepository.Delete(facilidade);
            }
            
            foreach (var facilidade in request.Facilidades)
            {
                var profFacilidade = new ProfissionalFacilidade(entity.Id, facilidade);
                await _facilidadeRepository.AddAsync(profFacilidade);
            }

            await _uow.CommitAsync();

            // Buscar o profissional atualizado com todas as relações
            var profissionalAtualizado = await _repository.GetCompletoAsync(entity.Id);
            return _mapper.Map<ProfissionalResponse>(profissionalAtualizado);
        }
    }
}
