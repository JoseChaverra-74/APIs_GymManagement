using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Repositories;

public interface ITrainerRepository : IGenericRepository<Trainer>
{
    Task<Trainer?> GetByEmailAsync(string email);
}
