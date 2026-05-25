using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Services;

public interface ITrainerService
{
    Task<IEnumerable<Trainer>> GetAllAsync();
    Task<Trainer?> GetByIdAsync(int id);
    Task<Trainer> CreateAsync(Trainer trainer);
    Task UpdateAsync(int id, Trainer trainer);
    Task DeleteAsync(int id);
}
