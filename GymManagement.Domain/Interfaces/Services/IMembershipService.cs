using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Services;

public interface IMembershipService
{
    Task<IEnumerable<Membership>> GetAllAsync();
    Task<Membership?> GetByIdAsync(int id);
    Task<Membership> CreateAsync(Membership membership);
    Task UpdateAsync(int id, Membership membership);
    Task DeleteAsync(int id);
}
