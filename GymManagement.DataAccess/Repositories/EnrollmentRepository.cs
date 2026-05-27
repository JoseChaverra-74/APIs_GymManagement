using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByClassAndMemberAsync(int gymClassId, int memberId)
    {
        return await _dbSet.AnyAsync(e =>
            e.GymClassId == gymClassId &&
            e.MemberId == memberId &&
            e.Status == EnrollmentStatus.Active);
    }

    public async Task<int> CountActiveByClassAsync(int gymClassId)
    {
        return await _dbSet.CountAsync(e =>
            e.GymClassId == gymClassId &&
            e.Status == EnrollmentStatus.Active);
    }

    public async Task<IEnumerable<Enrollment>> GetByClassAsync(int gymClassId)
    {
        return await _dbSet
            .Include(e => e.Member)
            .Include(e => e.GymClass)
            .Where(e => e.GymClassId == gymClassId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Enrollment>> GetByMemberAsync(int memberId)
    {
        return await _dbSet
            .Include(e => e.Member)
            .Include(e => e.GymClass)
                .ThenInclude(g => g.Trainer)
            .Where(e => e.MemberId == memberId)
            .ToListAsync();
    }

    public async Task<Enrollment?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(e => e.Member)
            .Include(e => e.GymClass)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
}
