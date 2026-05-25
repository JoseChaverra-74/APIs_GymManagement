using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Repositories;

public interface IGymClassRepository : IGenericRepository<GymClass>
{
    Task<GymClass?> GetByIdWithTrainerAsync(int id);
    Task<IEnumerable<GymClass>> GetAllWithTrainerAsync();
    Task<IEnumerable<GymClass>> GetByTrainerIdAsync(int trainerId);
}
