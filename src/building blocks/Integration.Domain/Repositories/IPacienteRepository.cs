using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IPacienteRepository : IGenericRepository<Paciente>
    {
        Task<Paciente> GetByCpfAsync(string cpf);
        Task<bool> ExisteCpfAsync(string cpf);
        Task<IEnumerable<Paciente>> GetPacientesAtivosAsync();
        Task<IEnumerable<Paciente>> GetPacientesPorStatusAsync(StatusPaciente status);
        Task<Paciente> GetByNumeroCooperadoAsync(string numeroCooperado);
        Task<IEnumerable<Paciente>> GetByNomeAsync(string nome);
        Task<IEnumerable<Paciente>> GetByCpfOrNomeAsync(string termo);
    }
}
