using AlphaProject.Domain.Entities;

namespace AlphaProject.Domain.Interfaces
{
    public interface IProjectionStrategy
    {
        // Devuelve una lista de filas, representando cada periodo (mes/día) de la proyección.
        Task<IEnumerable<ProjectionRow>> CalculateAsync(ProjectionParameters parameters);
    }
}