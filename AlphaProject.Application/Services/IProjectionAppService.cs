using AlphaProject.Application.DTOs;

namespace AlphaProject.Application.Services
{
    public interface IProjectionAppService
    {
        Task<IEnumerable<ProjectionRowDto>> GenerateProjectionAsync(CalculateProjectionRequest request);
    }
}