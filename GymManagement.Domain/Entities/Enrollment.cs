using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities;

public class Enrollment : AuditBase
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public int GymClassId { get; set; }
    public GymClass GymClass { get; set; } = null!;

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
}
