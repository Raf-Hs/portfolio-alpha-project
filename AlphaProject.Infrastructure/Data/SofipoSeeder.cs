using AlphaProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlphaProject.Infrastructure.Data;

public static class SofipoSeeder
{
    public static async Task SeedAsync(AlphaDbContext context)
    {
        try
        {
            if (await context.Institutions.AnyAsync())
            {
                Console.WriteLine("--- La tabla Institutions ya tiene datos, saltando seeding ---");
                return;
            }

            Console.WriteLine("--- Iniciando seeding de instituciones ---");

            var institutions = new List<Institution>
            {
                new Institution
                {
                    Name = "Nu",
                    InterestRules = new List<TieredInterestRule>
                    {
                        new TieredInterestRule
                        {
                            LimitAmount = 25000,
                            PrimaryRate = 13.00m,
                            FallbackRate = 7.00m
                        }
                    }
                },
                new Institution
                {
                    Name = "Mercado Pago",
                    InterestRules = new List<TieredInterestRule>
                    {
                        new TieredInterestRule
                        {
                            LimitAmount = 100000,
                            PrimaryRate = 13.00m,
                            FallbackRate = 0.00m
                        }
                    }
                },
                new Institution
                {
                    Name = "DiDi",
                    InterestRules = new List<TieredInterestRule>
                    {
                        new TieredInterestRule
                        {
                            LimitAmount = 10000,
                            PrimaryRate = 15.00m,
                            FallbackRate = 0.00m
                        }
                    }
                }
            };

            context.Institutions.AddRange(institutions);
            await context.SaveChangesAsync();
            Console.WriteLine("--- Seeding completado exitosamente ---");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--- Error en Seeder: {ex.Message} ---");
            Console.WriteLine($"--- StackTrace: {ex.StackTrace} ---");
        }
    }
}
