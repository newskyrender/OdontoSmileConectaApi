using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IAgendamentoRepository : IGenericRepository<Agendamento>
    {
        Task<IEnumerable<Agendamento>> GetPorDataAsync(DateTime data);
        Task<IEnumerable<Agendamento>> GetPorProfissionalAsync(Guid profissionalId);
        Task<IEnumerable<Agendamento>> GetPorPacienteAsync(Guid pacienteId);
        Task<IEnumerable<Agendamento>> GetPorStatusAsync(StatusAgendamento status);
        Task<IEnumerable<Agendamento>> GetPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Agendamento>> GetAgendamentosHojeAsync();
        Task<IEnumerable<Agendamento>> GetPorProfissionalEDataAsync(Guid profissionalId, DateTime data);
        Task<bool> VerificarDisponibilidadeAsync(Guid profissionalId, DateTime data, TimeSpan horario, int duracao);
        Task<IEnumerable<Agendamento>> GetConflitosAsync(Guid profissionalId, DateTime data, TimeSpan horario, int duracao);
    }
}
