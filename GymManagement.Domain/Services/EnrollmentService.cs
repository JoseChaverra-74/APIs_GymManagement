using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Helpers;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly GymValidationHelper _validationHelper;
    private readonly ILogger<EnrollmentService> _logger;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        GymValidationHelper validationHelper,
        ILogger<EnrollmentService> logger)
    {
        _enrollmentRepository = enrollmentRepository;
        _validationHelper = validationHelper;
        _logger = logger;
    }

    public async Task<Enrollment> EnrollAsync(int gymClassId, int memberId)
    {
        var gymClass = await _validationHelper.ValidateClassScheduledAsync(gymClassId);
        await _validationHelper.ValidateActiveMemberAsync(memberId);

        if (await _enrollmentRepository.ExistsByClassAndMemberAsync(gymClassId, memberId))
            throw new InvalidOperationException(
                "El socio ya está inscrito en esta clase");

        var activeCount = await _enrollmentRepository.CountActiveByClassAsync(gymClassId);
        if (activeCount >= gymClass.MaxCapacity)
            throw new InvalidOperationException(
                "La clase ha alcanzado su capacidad máxima");

        var enrollment = new Enrollment
        {
            GymClassId = gymClassId,
            MemberId = memberId,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Active
        };

        _logger.LogInformation(
            "Enrolling member {MemberId} in class {GymClassId}",
            memberId, gymClassId);
        return await _enrollmentRepository.CreateAsync(enrollment);
    }

    public async Task<IEnumerable<Enrollment>> GetByClassAsync(int gymClassId)
    {
        await _validationHelper.ValidateClassExistsAsync(gymClassId);
        return await _enrollmentRepository.GetByClassAsync(gymClassId);
    }

    public async Task<IEnumerable<Enrollment>> GetByMemberAsync(int memberId)
    {
        await _validationHelper.ValidateMemberExistsAsync(memberId);
        return await _enrollmentRepository.GetByMemberAsync(memberId);
    }

    public async Task CancelAsync(int gymClassId, int enrollmentId)
    {
        await _validationHelper.ValidateClassScheduledAsync(gymClassId);

        var enrollment = await _enrollmentRepository.GetByIdWithDetailsAsync(enrollmentId);
        if (enrollment == null || enrollment.GymClassId != gymClassId)
            throw new KeyNotFoundException(
                $"No se encontró la inscripción con ID {enrollmentId} en la clase {gymClassId}");

        if (enrollment.Status != EnrollmentStatus.Active)
            throw new InvalidOperationException(
                "Solo se pueden cancelar inscripciones activas");

        enrollment.Status = EnrollmentStatus.Cancelled;

        _logger.LogInformation(
            "Cancelling enrollment {EnrollmentId} for class {GymClassId}",
            enrollmentId, gymClassId);
        await _enrollmentRepository.UpdateAsync(enrollment);
    }
}
