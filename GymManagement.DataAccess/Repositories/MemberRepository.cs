using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{
    public MemberRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<Member?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m => m.Email.ToLower() == email.ToLower());
    }

    public async Task<Member?> GetByIdWithMembershipAsync(int id)
    {
        return await _dbSet
            .Include(m => m.Membership)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Member>> GetAllWithMembershipAsync()
    {
        return await _dbSet
            .Include(m => m.Membership)
            .ToListAsync();
    }
}
