namespace GymManagement.Domain.Entities;

public class Trainer : AuditBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Specialty { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<GymClass> GymClasses { get; set; } = new List<GymClass>();
}
