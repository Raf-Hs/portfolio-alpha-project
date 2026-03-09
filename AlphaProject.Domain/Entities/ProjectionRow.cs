namespace AlphaProject.Domain.Entities
{
    /// <summary>
    /// Representa el estado financiero en un periodo específico de la proyección.
    /// Inmutable tras su creación para evitar efectos secundarios.
    /// </summary>
    public class ProjectionRow
    {
        public int Period { get; private set; }
        public decimal StartingBalance { get; private set; }
        public decimal InterestEarned { get; private set; }
        public decimal EndingBalance { get; private set; }

        public ProjectionRow(int period, decimal startingBalance, decimal interestEarned, decimal endingBalance)
        {
            Period = period;
            StartingBalance = startingBalance;
            InterestEarned = interestEarned;
            EndingBalance = endingBalance;
        }
    }
}