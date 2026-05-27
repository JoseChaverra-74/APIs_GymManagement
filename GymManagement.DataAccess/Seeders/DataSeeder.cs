using GymManagement.DataAccess.Context;
using GymManagement.Domain.Entities;
using GymManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(GymDbContext context)
    {
        if (await context.Members.AnyAsync()) return;

        var memberships = new List<Membership>
        {
            new() { Name = "Plan Básico", Type = MembershipType.Basic, MonthlyPrice = 49.99m, DurationDays = 30, Description = "Acceso a zona de cardio y pesas" },
            new() { Name = "Plan Estándar", Type = MembershipType.Standard, MonthlyPrice = 79.99m, DurationDays = 30, Description = "Incluye clases grupales básicas" },
            new() { Name = "Plan Premium", Type = MembershipType.Premium, MonthlyPrice = 119.99m, DurationDays = 30, Description = "Acceso ilimitado y clases premium" }
        };
        context.Memberships.AddRange(memberships);
        await context.SaveChangesAsync();

        var trainers = new List<Trainer>
        {
            new() { FirstName = "Laura", LastName = "Gómez", Email = "laura.gomez@gym.com", Phone = "3001110001", Specialty = "Yoga" },
            new() { FirstName = "Carlos", LastName = "Ruiz", Email = "carlos.ruiz@gym.com", Phone = "3001110002", Specialty = "CrossFit" },
            new() { FirstName = "María", LastName = "López", Email = "maria.lopez@gym.com", Phone = "3001110003", Specialty = "Spinning" },
            new() { FirstName = "Andrés", LastName = "Vega", Email = "andres.vega@gym.com", Phone = "3001110004", Specialty = "Funcional" },
            new() { FirstName = "Diana", LastName = "Mora", Email = "diana.mora@gym.com", Phone = "3001110005", Specialty = "Pilates" }
        };
        context.Trainers.AddRange(trainers);
        await context.SaveChangesAsync();

        var members = new List<Member>
        {
            new() { FirstName = "Juan", LastName = "Pérez", Email = "juan.perez@email.com", Phone = "3101000001", DateOfBirth = new DateTime(1995, 3, 12), MembershipId = memberships[0].Id },
            new() { FirstName = "Ana", LastName = "Torres", Email = "ana.torres@email.com", Phone = "3101000002", DateOfBirth = new DateTime(1998, 7, 22), MembershipId = memberships[1].Id },
            new() { FirstName = "Luis", LastName = "Ramírez", Email = "luis.ramirez@email.com", Phone = "3101000003", DateOfBirth = new DateTime(1992, 11, 5), MembershipId = memberships[2].Id },
            new() { FirstName = "Sofía", LastName = "Castro", Email = "sofia.castro@email.com", Phone = "3101000004", DateOfBirth = new DateTime(2000, 1, 18), MembershipId = memberships[1].Id },
            new() { FirstName = "Diego", LastName = "Herrera", Email = "diego.herrera@email.com", Phone = "3101000005", DateOfBirth = new DateTime(1997, 9, 30), MembershipId = memberships[0].Id },
            new() { FirstName = "Valentina", LastName = "Ríos", Email = "valentina.rios@email.com", Phone = "3101000006", DateOfBirth = new DateTime(1999, 4, 14), MembershipId = memberships[2].Id },
            new() { FirstName = "Camilo", LastName = "Ortiz", Email = "camilo.ortiz@email.com", Phone = "3101000007", DateOfBirth = new DateTime(1994, 12, 8), MembershipId = memberships[1].Id },
            new() { FirstName = "Isabella", LastName = "Mejía", Email = "isabella.mejia@email.com", Phone = "3101000008", DateOfBirth = new DateTime(2001, 6, 25), MembershipId = memberships[0].Id },
            new() { FirstName = "Felipe", LastName = "Salazar", Email = "felipe.salazar@email.com", Phone = "3101000009", DateOfBirth = new DateTime(1993, 8, 3), MembershipId = memberships[2].Id },
            new() { FirstName = "Mariana", LastName = "Duque", Email = "mariana.duque@email.com", Phone = "3101000010", DateOfBirth = new DateTime(1996, 2, 27), MembershipId = memberships[1].Id },
            new() { FirstName = "Sebastián", LastName = "Aguilar", Email = "sebastian.aguilar@email.com", Phone = "3101000011", DateOfBirth = new DateTime(1991, 10, 16), MembershipId = memberships[0].Id },
            new() { FirstName = "Paula", LastName = "Giraldo", Email = "paula.giraldo@email.com", Phone = "3101000012", DateOfBirth = new DateTime(2002, 5, 9), MembershipId = memberships[2].Id }
        };
        context.Members.AddRange(members);
        await context.SaveChangesAsync();

        var baseDate = DateTime.UtcNow.Date.AddDays(1);
        var gymClasses = new List<GymClass>
        {
            new() { Name = "Yoga Matutino", Description = "Sesión de yoga nivel intermedio", TrainerId = trainers[0].Id, ScheduledDate = baseDate, StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(7, 0, 0), MaxCapacity = 15 },
            new() { Name = "CrossFit Intenso", Description = "WOD de alta intensidad", TrainerId = trainers[1].Id, ScheduledDate = baseDate, StartTime = new TimeSpan(7, 30, 0), EndTime = new TimeSpan(8, 30, 0), MaxCapacity = 12 },
            new() { Name = "Spinning Express", Description = "Cardio en bicicleta", TrainerId = trainers[2].Id, ScheduledDate = baseDate, StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(19, 0, 0), MaxCapacity = 20 },
            new() { Name = "Funcional Tarde", Description = "Entrenamiento funcional", TrainerId = trainers[3].Id, ScheduledDate = baseDate.AddDays(1), StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(18, 0, 0), MaxCapacity = 18 },
            new() { Name = "Pilates Básico", Description = "Fortalecimiento y flexibilidad", TrainerId = trainers[4].Id, ScheduledDate = baseDate.AddDays(1), StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), MaxCapacity = 10 },
            new() { Name = "Yoga Nocturno", Description = "Relajación y estiramiento", TrainerId = trainers[0].Id, ScheduledDate = baseDate.AddDays(2), StartTime = new TimeSpan(19, 0, 0), EndTime = new TimeSpan(20, 0, 0), MaxCapacity = 15 },
            new() { Name = "CrossFit Avanzado", Description = "Para atletas experimentados", TrainerId = trainers[1].Id, ScheduledDate = baseDate.AddDays(2), StartTime = new TimeSpan(6, 30, 0), EndTime = new TimeSpan(7, 45, 0), MaxCapacity = 10 },
            new() { Name = "Spinning Premium", Description = "Sesión con música en vivo", TrainerId = trainers[2].Id, ScheduledDate = baseDate.AddDays(3), StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(19, 30, 0), MaxCapacity = 20 }
        };
        context.GymClasses.AddRange(gymClasses);
        await context.SaveChangesAsync();

        var enrollments = new List<Enrollment>
        {
            new() { MemberId = members[0].Id, GymClassId = gymClasses[0].Id },
            new() { MemberId = members[1].Id, GymClassId = gymClasses[0].Id },
            new() { MemberId = members[2].Id, GymClassId = gymClasses[1].Id },
            new() { MemberId = members[3].Id, GymClassId = gymClasses[1].Id },
            new() { MemberId = members[4].Id, GymClassId = gymClasses[2].Id },
            new() { MemberId = members[5].Id, GymClassId = gymClasses[2].Id },
            new() { MemberId = members[6].Id, GymClassId = gymClasses[3].Id },
            new() { MemberId = members[7].Id, GymClassId = gymClasses[4].Id },
            new() { MemberId = members[8].Id, GymClassId = gymClasses[5].Id },
            new() { MemberId = members[9].Id, GymClassId = gymClasses[6].Id }
        };
        context.Enrollments.AddRange(enrollments);
        await context.SaveChangesAsync();
    }
}
