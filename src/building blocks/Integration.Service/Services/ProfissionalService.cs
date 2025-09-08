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

        public async Task<IEnumerable<ICommandResult>> GetByNome(string nome)
        {
            var entities = await _repository.GetByNomeAsync(nome);

            if (!entities.Any()) AddNotification("Alert", $"Nenhum profissional encontrado com o nome contendo '{nome}'");

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

        public async Task<ICommandResult> HandleDto(ProfissionalRegisterRequestDto requestDto)
        {
            // Converter DTO para Request interno
            var request = ConvertDtoToRequest(requestDto);
            
            // Usar o método Handle existente
            return await Handle(request);
        }

        private ProfissionalRegisterRequest ConvertDtoToRequest(ProfissionalRegisterRequestDto dto)
        {
            var request = new ProfissionalRegisterRequest
            {
                // Dados Pessoais
                NomeCompleto = dto.NomeCompleto,
                Cpf = dto.Cpf,
                DataNascimento = dto.DataNascimento,
                Sexo = ConvertStringToSexo(dto.Sexo),
                EmailProfissional = dto.EmailProfissional,
                Celular = dto.Celular,
                TelefoneAdicional = dto.TelefoneAdicional,

                // Dados Profissionais
                Cro = dto.Cro,
                DataFormatura = dto.DataFormatura,
                UniversidadeFormacao = dto.UniversidadeFormacao,
                TempoExperiencia = ConvertStringToTempoExperiencia(dto.TempoExperiencia),
                OutrasEspecialidades = dto.OutrasEspecialidades,
                Especialidades = ConvertStringListToEspecialidades(dto.Especialidades),

                // Dados do Consultório
                NomeConsultorio = dto.NomeConsultorio,
                Cnpj = dto.Cnpj,
                TelefoneConsultorio = dto.TelefoneConsultorio,
                CepConsultorio = dto.Cep,
                EnderecoConsultorio = dto.EnderecoCompleto,
                BairroConsultorio = dto.Bairro,
                CidadeConsultorio = dto.Cidade,
                EstadoConsultorio = dto.Estado,
                NumeroCadeiras = ConvertStringToNumeroCadeiras(dto.NumeroCadeiras),
                OutrosEquipamentos = dto.OutrosEquipamentos,
                ObservacoesConsultorio = dto.ObservacoesConsultorio,
                Equipamentos = ConvertStringListToEquipamentos(dto.Equipamentos),
                Facilidades = ConvertStringListToFacilidades(dto.Facilidades),

                // Horários
                SegundaSextaInicio = ParseTimeSpan(dto.HorarioFuncionamento?.SegundaSexta?.Inicio),
                SegundaSextaFim = ParseTimeSpan(dto.HorarioFuncionamento?.SegundaSexta?.Fim),
                SabadoInicio = ParseTimeSpan(dto.HorarioFuncionamento?.Sabado?.Inicio),
                SabadoFim = ParseTimeSpan(dto.HorarioFuncionamento?.Sabado?.Fim),
                DomingoInicio = ParseTimeSpan(dto.HorarioFuncionamento?.Domingo?.Inicio),
                DomingoFim = ParseTimeSpan(dto.HorarioFuncionamento?.Domingo?.Fim),
                TempoMedioConsulta = ConvertStringToTempoConsulta(dto.TempoMedioConsulta),

                // Dados Bancários
                Banco = dto.DadosBancarios?.Banco,
                TipoConta = ConvertStringToTipoConta(dto.DadosBancarios?.TipoConta),
                Agencia = dto.DadosBancarios?.Agencia,
                Conta = dto.DadosBancarios?.Conta,
                NomeTitular = dto.DadosBancarios?.NomeTitular,
                CpfTitular = dto.DadosBancarios?.CpfTitular,

                // Termos
                TermosUso = dto.TermosAceitos?.TermosUso ?? false,
                CodigoEtica = dto.TermosAceitos?.CodigoEtica ?? false,
                Responsabilidade = dto.TermosAceitos?.Responsabilidade ?? false,
                DadosPessoais = dto.TermosAceitos?.DadosPessoais ?? false,
                Marketing = dto.TermosAceitos?.Marketing ?? false
            };

            return request;
        }

        private Sexo ConvertStringToSexo(string sexo)
        {
            return sexo?.ToLower() switch
            {
                "masculino" or "m" => Sexo.Masculino,
                "feminino" or "f" => Sexo.Feminino,
                "outro" => Sexo.Outro,
                _ => Sexo.NaoInformar
            };
        }

        private TempoExperiencia ConvertStringToTempoExperiencia(string tempo)
        {
            return tempo?.ToLower()?.Trim() switch
            {
                "menos 1 ano" or "menos de 1 ano" => TempoExperiencia.Menos1Ano,
                "1-5 anos" or "entre 1 e 5 anos" => TempoExperiencia.Entre1e5Anos,
                "6-10 anos" or "entre 6 e 10 anos" => TempoExperiencia.Entre6e10Anos,
                "11-20 anos" or "entre 11 e 20 anos" => TempoExperiencia.Entre11e20Anos,
                "mais de 20 anos" or "20+ anos" => TempoExperiencia.Mais20Anos,
                _ => TempoExperiencia.Menos1Ano
            };
        }

        private NumeroCadeiras ConvertStringToNumeroCadeiras(string numero)
        {
            return numero?.ToLower()?.Trim() switch
            {
                "1 cadeira" or "uma cadeira" => NumeroCadeiras.UmaCadeira,
                "2 cadeiras" or "duas cadeiras" => NumeroCadeiras.DuasCadeiras,
                "3 cadeiras" or "três cadeiras" => NumeroCadeiras.TresCadeiras,
                "4 cadeiras" or "quatro cadeiras" => NumeroCadeiras.QuatroCadeiras,
                "5+ cadeiras" or "cinco ou mais" => NumeroCadeiras.CincoOuMaisCadeiras,
                _ => NumeroCadeiras.UmaCadeira
            };
        }

        private TempoConsulta? ConvertStringToTempoConsulta(string tempo)
        {
            if (string.IsNullOrEmpty(tempo)) return null;
            
            return tempo.ToLower().Trim() switch
            {
                "30 minutos" or "trinta minutos" => TempoConsulta.TrintaMinutos,
                "45 minutos" or "quarenta e cinco minutos" => TempoConsulta.QuarentaCincoMinutos,
                "60 minutos" or "sessenta minutos" or "1 hora" => TempoConsulta.SessentaMinutos,
                "90 minutos" or "noventa minutos" or "1h30" => TempoConsulta.NoventaMinutos,
                "120 minutos" or "cento e vinte minutos" or "2 horas" => TempoConsulta.CentoVinteMinutos,
                _ => TempoConsulta.TrintaMinutos
            };
        }

        private TipoConta? ConvertStringToTipoConta(string tipo)
        {
            if (string.IsNullOrEmpty(tipo)) return null;
            
            return tipo.ToLower().Trim() switch
            {
                "conta corrente" or "contacorrente" => TipoConta.ContaCorrente,
                "conta poupança" or "conta poupanca" or "poupança" => TipoConta.ContaPoupanca,
                _ => TipoConta.ContaCorrente
            };
        }

        private List<Especialidade> ConvertStringListToEspecialidades(List<string> especialidades)
        {
            if (especialidades == null) return new List<Especialidade>();
            
            var result = new List<Especialidade>();
            foreach (var esp in especialidades)
            {
                var especialidade = esp.ToLower().Trim() switch
                {
                    "ortodontia" => Especialidade.Ortodontia,
                    "implantodontia" => Especialidade.Implantodontia,
                    "endodontia" => Especialidade.Endodontia,
                    "prótese" or "protese" => Especialidade.Protese,
                    "periodontia" => Especialidade.Periodontia,
                    "cirurgia oral" => Especialidade.CirurgiaOral,
                    "dentística" or "dentistica" => Especialidade.Dentistica,
                    "clínica geral" or "clinica geral" => Especialidade.ClinicaGeral,
                    _ => (Especialidade?)null
                };
                
                if (especialidade.HasValue)
                    result.Add(especialidade.Value);
            }
            
            return result;
        }

        private List<Equipamento> ConvertStringListToEquipamentos(List<string> equipamentos)
        {
            if (equipamentos == null) return new List<Equipamento>();
            
            var result = new List<Equipamento>();
            foreach (var eq in equipamentos)
            {
                var equipamento = eq.ToLower().Trim() switch
                {
                    "scanner itero" or "itero" => Equipamento.ScannerItero,
                    "scanner medit" or "medit" => Equipamento.ScannerMedit,
                    "scanner 3shape" or "3shape" => Equipamento.Scanner3Shape,
                    "scanner cerec" or "cerec" => Equipamento.ScannerCerec,
                    "raio-x digital" or "raio x digital" => Equipamento.RaioXDigital,
                    "panorâmica" or "panoramica" => Equipamento.Panoramica,
                    "tomografia" => Equipamento.Tomografia,
                    "laser terapêutico" or "laser terapeutico" => Equipamento.LaserTerapeutico,
                    _ => (Equipamento?)null
                };
                
                if (equipamento.HasValue)
                    result.Add(equipamento.Value);
            }
            
            return result;
        }

        private List<Facilidade> ConvertStringListToFacilidades(List<string> facilidades)
        {
            if (facilidades == null) return new List<Facilidade>();
            
            var result = new List<Facilidade>();
            foreach (var fac in facilidades)
            {
                var facilidade = fac.ToLower().Trim() switch
                {
                    "estacionamento" => Facilidade.Estacionamento,
                    "acessibilidade" => Facilidade.Acessibilidade,
                    "ar condicionado" => Facilidade.ArCondicionado,
                    "wifi" or "wi-fi" => Facilidade.Wifi,
                    _ => (Facilidade?)null
                };
                
                if (facilidade.HasValue)
                    result.Add(facilidade.Value);
            }
            
            return result;
        }

        private TimeSpan? ParseTimeSpan(string time)
        {
            if (string.IsNullOrEmpty(time)) return null;
            
            if (TimeSpan.TryParse(time, out var result))
                return result;
                
            return null;
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
