using AutoMapper;
using FluentValidator;
using Integration.Domain.Entities;
using Integration.Domain.Http.Response;
using Integration.Domain.Repositories;
using Integration.Domain.Common;
using Integration.Domain.Enums;
using Integration.Infrastructure.Transactions;

namespace Integration.Service.Services
{
    public class DashboardService : Notifiable
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public DashboardService(IPacienteRepository pacienteRepository,
            IAgendamentoRepository agendamentoRepository, IMapper mapper, IUow uow)
        {
            _pacienteRepository = pacienteRepository;
            _agendamentoRepository = agendamentoRepository;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ICommandResult> GetTotais()
        {
            var totalPacientes = await _pacienteRepository.GetCountAsync();
            var consultasHoje = await _agendamentoRepository.GetConsultasHojeCountAsync();

            var response = new DashboardTotaisResponse
            {
                TotalPacientes = totalPacientes,
                ConsultasHoje = consultasHoje
            };

            return response;
        }

        public async Task<IEnumerable<ICommandResult>> GetConsultasHoje()
        {
            var consultas = await _agendamentoRepository.GetConsultasHojeAsync();

            if (!consultas.Any()) 
                AddNotification("Info", "Nenhuma consulta agendada para hoje");

            var response = consultas.Select(x => new ConsultaHojeResponse
            {
                Id = x.Id,
                NomePaciente = x.Paciente?.NomeCompleto ?? "N/A",
                NomeProfissional = x.Profissional?.NomeCompleto ?? "N/A",
                DataConsulta = x.DataAgendamento,
                Status = x.Status.ToString(),
                TipoConsulta = x.Servico.ToString(),
                Observacoes = x.Observacoes
            });

            return response.Cast<ICommandResult>();
        }

        public async Task<ICommandResult> GetStatusRapido()
        {
            var hoje = DateTime.Today;
            var inicioSemana = hoje.AddDays(-(int)hoje.DayOfWeek);
            var fimSemana = inicioSemana.AddDays(6);

            // Consultas de hoje
            var consultasHoje = await _agendamentoRepository.GetConsultasHojeAsync();
            var consultasConcluidas = consultasHoje.Count(x => x.Status == StatusAgendamento.Realizado);
            var totalHoje = consultasHoje.Count();

            // Pendências (consultas aguardando)
            var pendencias = await _agendamentoRepository.GetConsultasPendentesAsync();

            // Cancelamentos da semana
            var cancelamentos = await _agendamentoRepository.GetCancelamentosSemanaAsync(inicioSemana, fimSemana);

            var response = new StatusRapidoResponse
            {
                ConsultasConcluidas = new ConsultasConcluidasInfo
                {
                    Concluidas = consultasConcluidas,
                    TotalHoje = totalHoje
                },
                Pendencias = new PendenciasInfo
                {
                    Quantidade = pendencias.Count()
                },
                Cancelamentos = new CancelamentosInfo
                {
                    Quantidade = cancelamentos.Count()
                }
            };

            return response;
        }

        public async Task<IEnumerable<ICommandResult>> GetPacientesRecentes()
        {
            var pacientes = await _pacienteRepository.GetPacientesRecentesAsync();

            if (!pacientes.Any()) 
                AddNotification("Info", "Nenhum paciente encontrado");

            var response = new List<PacienteRecenteResponse>();

            foreach (var paciente in pacientes)
            {
                var ultimaConsulta = await _agendamentoRepository.GetUltimaConsultaPacienteAsync(paciente.Id);
                var proximaConsulta = await _agendamentoRepository.GetProximaConsultaPacienteAsync(paciente.Id);

                response.Add(new PacienteRecenteResponse
                {
                    Id = paciente.Id,
                    Nome = paciente.NomeCompleto,
                    DataUltimaConsulta = ultimaConsulta?.DataAgendamento,
                    DataProximaConsulta = proximaConsulta?.DataAgendamento,
                    Status = paciente.Status == StatusPaciente.Ativo ? "Ativo" : "Inativo"
                });
            }

            return response.Cast<ICommandResult>();
        }
    }
}
