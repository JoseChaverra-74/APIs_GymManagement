using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Repositories;

public interface IMembershipRepository : IGenericRepository<Membership>
{
    Task<Membership?> GetByNameAsync(string name);
    Task<bool> HasActiveMembersAsync(int membershipId);
}
