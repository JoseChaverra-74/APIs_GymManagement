using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services;

public class MembershipService : IMembershipService
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly ILogger<MembershipService> _logger;

    public MembershipService(
        IMembershipRepository membershipRepository,
        ILogger<MembershipService> logger)
    {
        _membershipRepository = membershipRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Membership>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all memberships");
        return await _membershipRepository.GetAllAsync();
    }

    public async Task<Membership?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving membership with ID: {MembershipId}", id);
        return await _membershipRepository.GetByIdAsync(id);
    }

    public async Task<Membership> CreateAsync(Membership membership)
    {
        if (membership.MonthlyPrice <= 0)
            throw new InvalidOperationException("El precio mensual debe ser mayor a 0");

        if (membership.DurationDays <= 0)
            throw new InvalidOperationException("La duración debe ser mayor a 0 días");

        var existing = await _membershipRepository.GetByNameAsync(membership.Name);
        if (existing != null)
            throw new InvalidOperationException(
                $"Ya existe un plan con el nombre '{membership.Name}'");

        _logger.LogInformation("Creating membership: {MembershipName}", membership.Name);
        return await _membershipRepository.CreateAsync(membership);
    }

    public async Task UpdateAsync(int id, Membership membership)
    {
        var existing = await _membershipRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el plan con ID {id}");

        if (membership.MonthlyPrice <= 0)
            throw new InvalidOperationException("El precio mensual debe ser mayor a 0");

        if (membership.DurationDays <= 0)
            throw new InvalidOperationException("La duración debe ser mayor a 0 días");

        if (!string.Equals(existing.Name, membership.Name, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _membershipRepository.GetByNameAsync(membership.Name);
            if (duplicate != null)
                throw new InvalidOperationException(
                    $"Ya existe un plan con el nombre '{membership.Name}'");
        }

        existing.Name = membership.Name;
        existing.Type = membership.Type;
        existing.MonthlyPrice = membership.MonthlyPrice;
        existing.DurationDays = membership.DurationDays;
        existing.Description = membership.Description;
        existing.IsActive = membership.IsActive;

        _logger.LogInformation("Updating membership with ID: {MembershipId}", id);
        await _membershipRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _membershipRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el plan con ID {id}");

        if (await _membershipRepository.HasActiveMembersAsync(id))
            throw new InvalidOperationException(
                "No se puede eliminar un plan con socios asociados");

        _logger.LogInformation("Deleting membership with ID: {MembershipId}", id);
        await _membershipRepository.DeleteAsync(id);
    }
}
