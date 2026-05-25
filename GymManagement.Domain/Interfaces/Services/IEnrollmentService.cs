using GymManagement.Domain.Entities;

namespace GymManagement.Domain.Interfaces.Services;

public interface IEnrollmentService
{
    Task<Enrollment> EnrollAsync(int gymClassId, int memberId);
    Task<IEnumerable<Enrollment>> GetByClassAsync(int gymClassId);
    Task<IEnumerable<Enrollment>> GetByMemberAsync(int memberId);
    Task CancelAsync(int gymClassId, int enrollmentId);
}
