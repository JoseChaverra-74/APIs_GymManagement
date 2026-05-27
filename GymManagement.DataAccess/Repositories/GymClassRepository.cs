using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories;

public class GymClassRepository : GenericRepository<GymClass>, IGymClassRepository
{
    public GymClassRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<GymClass?> GetByIdWithTrainerAsync(int id)
    {
        return await _dbSet
            .Include(g => g.Trainer)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<IEnumerable<GymClass>> GetAllWithTrainerAsync()
    {
        return await _dbSet
            .Include(g => g.Trainer)
            .ToListAsync();
    }

    public async Task<IEnumerable<GymClass>> GetByTrainerIdAsync(int trainerId)
    {
        return await _dbSet
            .Include(g => g.Trainer)
            .Where(g => g.TrainerId == trainerId)
            .ToListAsync();
    }
}
