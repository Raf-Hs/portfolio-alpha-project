using AlphaProject.Domain.Entities;
using AlphaProject.Domain.Interfaces;

namespace AlphaProject.Domain.Services
{
    public class CompoundInterestStrategy : IProjectionStrategy
    {
        public Task<IEnumerable<ProjectionRow>> CalculateAsync(ProjectionParameters parameters)
        {
            // Pre-asignamos la capacidad de la lista por rendimiento (evita re-allocations en memoria)
            var rows = new List<ProjectionRow>(parameters.TermInMonths);
            decimal currentBalance = parameters.InitialCapital;

            // Regla 80/20: Para el MVP, asumimos capitalización mensual.
            // Si la tasa anual entra como porcentaje (ej. 15 para 15%), la convertimos a decimal y dividimos entre 12.
            // El sufijo 'm' es obligatorio en C# para forzar la evaluación como Decimal y no como Double.
            decimal monthlyInterestRate = (parameters.AnnualInterestRate / 100m) / 12m;

            for (int month = 1; month <= parameters.TermInMonths; month++)
            {
                decimal startingBalance = currentBalance;
                
                // Calculamos el interés del mes exacto
                decimal interestEarned = startingBalance * monthlyInterestRate;
                
                // Calculamos el balance final del periodo
                decimal endingBalance = startingBalance + interestEarned;

                rows.Add(new ProjectionRow(
                    period: month,
                    startingBalance: startingBalance,
                    interestEarned: interestEarned,
                    endingBalance: endingBalance
                ));

                // El balance final de este mes es el inicial del siguiente
                currentBalance = endingBalance;
            }

            // Retornamos de forma asíncrona simulada. 
            // Aunque esto es CPU-bound y síncrono por naturaleza, mantener la firma Task 
            // permite que futuras estrategias (que quizá consulten una DB o API externa para tasas dinámicas) no rompan la interfaz.
            return Task.FromResult<IEnumerable<ProjectionRow>>(rows);
        }
    }
}