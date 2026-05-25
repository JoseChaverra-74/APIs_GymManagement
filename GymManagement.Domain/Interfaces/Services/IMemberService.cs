using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Interfaces.Services;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<Member> CreateAsync(Member member);
    Task UpdateAsync(int id, Member member);
    Task DeleteAsync(int id);
    Task UpdateStatusAsync(int id, MemberStatus status);
}
