using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMembershipRepository _membershipRepository;
    private readonly ILogger<MemberService> _logger;

    public MemberService(
        IMemberRepository memberRepository,
        IMembershipRepository membershipRepository,
        ILogger<MemberService> logger)
    {
        _memberRepository = memberRepository;
        _membershipRepository = membershipRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all members");
        return await _memberRepository.GetAllWithMembershipAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving member with ID: {MemberId}", id);
        return await _memberRepository.GetByIdWithMembershipAsync(id);
    }

    public async Task<Member> CreateAsync(Member member)
    {
        var membership = await _membershipRepository.GetByIdAsync(member.MembershipId);
        if (membership == null || !membership.IsActive)
            throw new InvalidOperationException("La membresía seleccionada no es válida o está inactiva");

        var existingEmail = await _memberRepository.GetByEmailAsync(member.Email);
        if (existingEmail != null)
            throw new InvalidOperationException(
                $"Ya existe un socio con el email '{member.Email}'");

        member.Status = MemberStatus.Active;

        _logger.LogInformation("Creating member: {MemberEmail}", member.Email);
        return await _memberRepository.CreateAsync(member);
    }

    public async Task UpdateAsync(int id, Member member)
    {
        var existing = await _memberRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el socio con ID {id}");

        var membership = await _membershipRepository.GetByIdAsync(member.MembershipId);
        if (membership == null || !membership.IsActive)
            throw new InvalidOperationException("La membresía seleccionada no es válida o está inactiva");

        if (!string.Equals(existing.Email, member.Email, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _memberRepository.GetByEmailAsync(member.Email);
            if (duplicate != null)
                throw new InvalidOperationException(
                    $"Ya existe un socio con el email '{member.Email}'");
        }

        existing.FirstName = member.FirstName;
        existing.LastName = member.LastName;
        existing.Email = member.Email;
        existing.Phone = member.Phone;
        existing.DateOfBirth = member.DateOfBirth;
        existing.MembershipId = member.MembershipId;

        _logger.LogInformation("Updating member with ID: {MemberId}", id);
        await _memberRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _memberRepository.ExistsAsync(id))
            throw new KeyNotFoundException($"No se encontró el socio con ID {id}");

        _logger.LogInformation("Deleting member with ID: {MemberId}", id);
        await _memberRepository.DeleteAsync(id);
    }

    public async Task UpdateStatusAsync(int id, MemberStatus status)
    {
        var existing = await _memberRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el socio con ID {id}");

        existing.Status = status;

        _logger.LogInformation("Updating member status with ID: {MemberId}", id);
        await _memberRepository.UpdateAsync(existing);
    }
}
