using GymManagement.Domain.Enums;

namespace GymManagement.Domain.Entities;

public class GymClass : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime ScheduledDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int MaxCapacity { get; set; }
    public GymClassStatus Status { get; set; } = GymClassStatus.Scheduled;

    public int TrainerId { get; set; }
    public Trainer Trainer { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
