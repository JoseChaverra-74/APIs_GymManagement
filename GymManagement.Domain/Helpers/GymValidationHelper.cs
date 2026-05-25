using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Interfaces.Repositories;

namespace GymManagement.Domain.Helpers;

public class GymValidationHelper
{
    private readonly IGymClassRepository _gymClassRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly ITrainerRepository _trainerRepository;

    public GymValidationHelper(
        IGymClassRepository gymClassRepository,
        IMemberRepository memberRepository,
        IMembershipRepository membershipRepository,
        ITrainerRepository trainerRepository)
    {
        _gymClassRepository = gymClassRepository;
        _memberRepository = memberRepository;
        _membershipRepository = membershipRepository;
        _trainerRepository = trainerRepository;
    }

    public async Task<GymClass> ValidateClassExistsAsync(int gymClassId)
    {
        var gymClass = await _gymClassRepository.GetByIdAsync(gymClassId);
        if (gymClass == null)
            throw new KeyNotFoundException(
                $"No se encontró la clase con ID {gymClassId}");

        return gymClass;
    }

    public async Task<GymClass> ValidateClassScheduledAsync(int gymClassId)
    {
        var gymClass = await ValidateClassExistsAsync(gymClassId);

        if (gymClass.Status != GymClassStatus.Scheduled)
            throw new InvalidOperationException(
                "Solo se pueden inscribir socios en clases con estado Scheduled");

        return gymClass;
    }

    public async Task<Member> ValidateMemberExistsAsync(int memberId)
    {
        var member = await _memberRepository.GetByIdWithMembershipAsync(memberId);
        if (member == null)
            throw new KeyNotFoundException(
                $"No se encontró el socio con ID {memberId}");

        return member;
    }

    public async Task<Member> ValidateActiveMemberAsync(int memberId)
    {
        var member = await ValidateMemberExistsAsync(memberId);

        if (member.Status != MemberStatus.Active)
            throw new InvalidOperationException(
                "Solo los socios activos pueden inscribirse en clases");

        var membership = await _membershipRepository.GetByIdAsync(member.MembershipId);
        if (membership == null || !membership.IsActive)
            throw new InvalidOperationException(
                "El socio no tiene una membresía activa válida");

        return member;
    }

    public async Task<Trainer> ValidateActiveTrainerAsync(int trainerId)
    {
        var trainer = await _trainerRepository.GetByIdAsync(trainerId);
        if (trainer == null)
            throw new KeyNotFoundException(
                $"No se encontró el entrenador con ID {trainerId}");

        if (!trainer.IsActive)
            throw new InvalidOperationException(
                "El entrenador debe estar activo para impartir clases");

        return trainer;
    }

    public static void ValidateSchedule(DateTime scheduledDate, TimeSpan startTime, TimeSpan endTime)
    {
        if (endTime <= startTime)
            throw new InvalidOperationException(
                "La hora de fin debe ser posterior a la hora de inicio");
    }

    public static void ValidateMaxCapacity(int maxCapacity)
    {
        if (maxCapacity < 1)
            throw new InvalidOperationException(
                "La capacidad máxima debe ser al menos 1");
    }
}
