using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories;

public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
{
    public MembershipRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<Membership?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> HasActiveMembersAsync(int membershipId)
    {
        return await _context.Members
            .AnyAsync(m => m.MembershipId == membershipId);
    }
}
