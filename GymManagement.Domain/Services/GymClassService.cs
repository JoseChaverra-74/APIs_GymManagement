using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using GymManagement.Domain.Helpers;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services;

public class GymClassService : IGymClassService
{
    private readonly IGymClassRepository _gymClassRepository;
    private readonly GymValidationHelper _validationHelper;
    private readonly ILogger<GymClassService> _logger;

    public GymClassService(
        IGymClassRepository gymClassRepository,
        GymValidationHelper validationHelper,
        ILogger<GymClassService> logger)
    {
        _gymClassRepository = gymClassRepository;
        _validationHelper = validationHelper;
        _logger = logger;
    }

    public async Task<IEnumerable<GymClass>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all gym classes");
        return await _gymClassRepository.GetAllWithTrainerAsync();
    }

    public async Task<GymClass?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving gym class with ID: {GymClassId}", id);
        return await _gymClassRepository.GetByIdWithTrainerAsync(id);
    }

    public async Task<IEnumerable<GymClass>> GetByTrainerIdAsync(int trainerId)
    {
        await _validationHelper.ValidateActiveTrainerAsync(trainerId);
        return await _gymClassRepository.GetByTrainerIdAsync(trainerId);
    }

    public async Task<GymClass> CreateAsync(GymClass gymClass)
    {
        await _validationHelper.ValidateActiveTrainerAsync(gymClass.TrainerId);
        GymValidationHelper.ValidateSchedule(
            gymClass.ScheduledDate, gymClass.StartTime, gymClass.EndTime);
        GymValidationHelper.ValidateMaxCapacity(gymClass.MaxCapacity);

        gymClass.Status = GymClassStatus.Scheduled;

        _logger.LogInformation("Creating gym class: {GymClassName}", gymClass.Name);
        return await _gymClassRepository.CreateAsync(gymClass);
    }

    public async Task UpdateAsync(int id, GymClass gymClass)
    {
        var existing = await _gymClassRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró la clase con ID {id}");

        if (existing.Status != GymClassStatus.Scheduled)
            throw new InvalidOperationException(
                "Solo se pueden editar clases en estado Scheduled");

        await _validationHelper.ValidateActiveTrainerAsync(gymClass.TrainerId);
        GymValidationHelper.ValidateSchedule(
            gymClass.ScheduledDate, gymClass.StartTime, gymClass.EndTime);
        GymValidationHelper.ValidateMaxCapacity(gymClass.MaxCapacity);

        existing.Name = gymClass.Name;
        existing.Description = gymClass.Description;
        existing.ScheduledDate = gymClass.ScheduledDate;
        existing.StartTime = gymClass.StartTime;
        existing.EndTime = gymClass.EndTime;
        existing.MaxCapacity = gymClass.MaxCapacity;
        existing.TrainerId = gymClass.TrainerId;

        _logger.LogInformation("Updating gym class with ID: {GymClassId}", id);
        await _gymClassRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _gymClassRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró la clase con ID {id}");

        if (existing.Status != GymClassStatus.Scheduled)
            throw new InvalidOperationException(
                "Solo se pueden eliminar clases en estado Scheduled");

        _logger.LogInformation("Deleting gym class with ID: {GymClassId}", id);
        await _gymClassRepository.DeleteAsync(id);
    }

    public async Task UpdateStatusAsync(int id, GymClassStatus status)
    {
        var existing = await _gymClassRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró la clase con ID {id}");

        existing.Status = status;

        _logger.LogInformation("Updating gym class status with ID: {GymClassId}", id);
        await _gymClassRepository.UpdateAsync(existing);
    }
}
