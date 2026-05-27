using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Repositories;

public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
{
    public TrainerRepository(GymDbContext context) : base(context)
    {
    }

    public async Task<Trainer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(t => t.Email.ToLower() == email.ToLower());
    }
}
