namespace AlphaProject.Domain.Entities
{
    /// <summary>
    /// Representa los datos inmutables de entrada para una proyección financiera.
    /// </summary>
    public class ProjectionParameters
    {
        public decimal InitialCapital { get; private set; }
        public decimal AnnualInterestRate { get; private set; }
        public int TermInMonths { get; private set; }

        // Constructor para forzar la validez de los datos desde la creación
        public ProjectionParameters(decimal initialCapital, decimal annualInterestRate, int termInMonths)
        {
            if (initialCapital <= 0) throw new ArgumentException("El capital debe ser mayor a cero.");
            if (annualInterestRate < 0) throw new ArgumentException("La tasa no puede ser negativa.");
            if (termInMonths <= 0) throw new ArgumentException("El plazo debe ser mayor a cero meses.");

            InitialCapital = initialCapital;
            AnnualInterestRate = annualInterestRate;
            TermInMonths = termInMonths;
        }
    }
}