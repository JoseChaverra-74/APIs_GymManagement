using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities;

public class Member : AuditBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public MemberStatus Status { get; set; } = MemberStatus.Active;

    public int MembershipId { get; set; }
    public Membership Membership { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
