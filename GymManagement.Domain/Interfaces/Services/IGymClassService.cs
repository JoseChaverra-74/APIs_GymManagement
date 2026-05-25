using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Interfaces.Services;

public interface IGymClassService
{
    Task<IEnumerable<GymClass>> GetAllAsync();
    Task<GymClass?> GetByIdAsync(int id);
    Task<IEnumerable<GymClass>> GetByTrainerIdAsync(int trainerId);
    Task<GymClass> CreateAsync(GymClass gymClass);
    Task UpdateAsync(int id, GymClass gymClass);
    Task DeleteAsync(int id);
    Task UpdateStatusAsync(int id, GymClassStatus status);
}
