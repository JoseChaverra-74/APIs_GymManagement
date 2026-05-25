using GymManagement.Domain.Entities;
using GymManagement.Domain.Interfaces.Repositories;
using GymManagement.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GymManagement.Domain.Services;

public class TrainerService : ITrainerService
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly ILogger<TrainerService> _logger;

    public TrainerService(
        ITrainerRepository trainerRepository,
        ILogger<TrainerService> logger)
    {
        _trainerRepository = trainerRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Trainer>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all trainers");
        return await _trainerRepository.GetAllAsync();
    }

    public async Task<Trainer?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving trainer with ID: {TrainerId}", id);
        return await _trainerRepository.GetByIdAsync(id);
    }

    public async Task<Trainer> CreateAsync(Trainer trainer)
    {
        var existing = await _trainerRepository.GetByEmailAsync(trainer.Email);
        if (existing != null)
            throw new InvalidOperationException(
                $"Ya existe un entrenador con el email '{trainer.Email}'");

        _logger.LogInformation("Creating trainer: {TrainerEmail}", trainer.Email);
        return await _trainerRepository.CreateAsync(trainer);
    }

    public async Task UpdateAsync(int id, Trainer trainer)
    {
        var existing = await _trainerRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el entrenador con ID {id}");

        if (!string.Equals(existing.Email, trainer.Email, StringComparison.OrdinalIgnoreCase))
        {
            var duplicate = await _trainerRepository.GetByEmailAsync(trainer.Email);
            if (duplicate != null)
                throw new InvalidOperationException(
                    $"Ya existe un entrenador con el email '{trainer.Email}'");
        }

        existing.FirstName = trainer.FirstName;
        existing.LastName = trainer.LastName;
        existing.Email = trainer.Email;
        existing.Phone = trainer.Phone;
        existing.Specialty = trainer.Specialty;
        existing.IsActive = trainer.IsActive;

        _logger.LogInformation("Updating trainer with ID: {TrainerId}", id);
        await _trainerRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _trainerRepository.ExistsAsync(id))
            throw new KeyNotFoundException($"No se encontró el entrenador con ID {id}");

        _logger.LogInformation("Deleting trainer with ID: {TrainerId}", id);
        await _trainerRepository.DeleteAsync(id);
    }
}
