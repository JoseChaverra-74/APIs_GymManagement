using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Interfaces.Repositories;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<bool> ExistsByClassAndMemberAsync(int gymClassId, int memberId);
    Task<int> CountActiveByClassAsync(int gymClassId);
    Task<IEnumerable<Enrollment>> GetByClassAsync(int gymClassId);
    Task<IEnumerable<Enrollment>> GetByMemberAsync(int memberId);
    Task<Enrollment?> GetByIdWithDetailsAsync(int id);
}
