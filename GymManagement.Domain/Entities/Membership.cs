using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities;

public class Membership : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public MembershipType Type { get; set; }
    public decimal MonthlyPrice { get; set; }
    public int DurationDays { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Member> Members { get; set; } = new List<Member>();
}
