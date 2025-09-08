using Integration.Domain.Entities;
using Integration.Domain.Enums;
using Integration.Domain.Repositories.Base;

namespace Integration.Domain.Repositories
{
    public interface IProfissionalRepository : IGenericRepository<Profissional>
    {
        Task<Profissional> GetByCpfAsync(string cpf);
        Task<Profissional> GetByCroAsync(string cro);
        Task<bool> ExisteCpfAsync(string cpf);
        Task<bool> ExisteCroAsync(string cro);
        Task<bool> ExisteEmailAsync(string email);
        Task<IEnumerable<Profissional>> GetProfissionaisAtivosAsync();
        Task<IEnumerable<Profissional>> GetPorStatusAprovacaoAsync(StatusAprovacao status);
        Task<IEnumerable<Profissional>> GetPorEspecialidadeAsync(Especialidade especialidade);
        Task<IEnumerable<Profissional>> GetByNomeAsync(string nome);
        Task<Profissional> GetComEspecialidadesAsync(Guid id);
        Task<Profissional> GetComEquipamentosAsync(Guid id);
        Task<Profissional> GetComFacilidadesAsync(Guid id);
        Task<Profissional> GetCompletoAsync(Guid id);
    }
}
